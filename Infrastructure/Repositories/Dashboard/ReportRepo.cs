using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Abstractions;
using Application.Contracts.Dashboard.Report;
using Application.DTOs;
using Application.Errors.Auth;
using Application.Errors.Report;
using Application.Helpers;
using Application.Helpers.FilterParams;
using Application.Interfaces.Dashboard;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Infrastructure.Repositories.Dashboard
{
    public class ReportRepo : IReportRepo
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportRepo(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<PagedList<ReportResponse>>> GetAllReports(BaseParams reportParams)
        {

            var userId = _httpContextAccessor.HttpContext?.User?.GetUserId();
            if (userId == null) return Result.Failure<PagedList<ReportResponse>>(AuthErrors.Unauthorized);


            var user = await _context.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.UserCategories)
                .FirstOrDefaultAsync(u => u.Id == userId);

           
            var isAdmin = user.UserRoles.Any(r => r.Role.Name == "Admin");



            var reportsQuery = _context.IssueReports.AsNoTracking()
                                                .Where(x => !x.IsDeleted &&
                                                            (string.IsNullOrEmpty(reportParams.Keyword) || x.Description.Contains(reportParams.Keyword)) &&
                                                            (reportParams.From == null || x.DateIssued >= reportParams.From) &&
                                                            (reportParams.To == null || x.DateIssued <= reportParams.To));
            if (!isAdmin)
            {
                var userCategoryIds = user.UserCategories.Select(uc => uc.CategoryId).ToList();
                reportsQuery = reportsQuery.Where(r => userCategoryIds.Contains(r.IssueCategoryId));
            }

            var projectedQuery = reportsQuery
                    .OrderByDescending(x => x.DateIssued)
                    .ProjectTo<ReportResponse>(_mapper.ConfigurationProvider);

            var result = await PagedList<ReportResponse>.CreateAsync(projectedQuery, reportParams);
            return Result.Success(result);

        }

        public async Task<Result<ReportResponse>> GetReportById(int id)
        {
            var report = await _context.IssueReports.Where(x => !x.IsDeleted && x.Id == id)
                                                    .ProjectTo<ReportResponse>(_mapper.ConfigurationProvider)
                                                    .FirstOrDefaultAsync();
            if (report == null)
                return Result.Failure<ReportResponse>(ReportErrors.NotFound);

            // Get status history
            var history = await _context.ReportStatusHistories
                .Where(h => h.IssueReportId == id)
                .OrderBy(h => h.CreatedAt)
                .Select(h => new ReportStatusHistoryDto
                {
                    Status = h.ReportStatus.ToString(),
                    ChangedBy = h.User.UserName, 
                    ChangedAt = h.CreatedAt,
                    Comment = h.Comment
                })
                .ToListAsync();

            report.StatusHistory = history;

            return Result.Success(report);

        }

        public async Task<Result> UpdateReportStatus(ChangeReportStatus changeReportStatus)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.GetUserId();
            if (userId == null) return Result.Failure<ReportResponse>(AuthErrors.Unauthorized);

            var report = await _context.IssueReports.FirstOrDefaultAsync(x => x.Id == changeReportStatus.ReportId);

            if (report == null) return Result.Failure<ReportResponse>(ReportErrors.NotFound);

            report.ReportStatus = changeReportStatus.Status;

            var newRecord = new IssueReportStatusHistory
            {
                IssueReportId = report.Id,
                CreatedAt = DateTime.UtcNow,
                ReportStatus = changeReportStatus.Status,
                UserId = userId,
                 Comment = changeReportStatus.Comment
            };

            await _context.ReportStatusHistories.AddAsync(newRecord);

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<ReportResponse>> DeleteReport(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userId == null) return Result.Failure<ReportResponse>(AuthErrors.Unauthorized);

            var report = await _context.IssueReports.FirstOrDefaultAsync(x => x.Id == id);

            if (report == null) return Result.Failure<ReportResponse>(ReportErrors.NotFound);

            report.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Result.Success(_mapper.Map<ReportResponse>(report));
        }

        public async Task<Result<int>> NumberOfReports()
        {
            var count = await _context.IssueReports.CountAsync();

            return Result.Success(count);
        }
        public async Task<ReportSummaryDto> GetReportSummaryAsync()
        {
            var summary = await _context.IssueReports
                .Where(x => !x.IsDeleted)
                .GroupBy(r => r.ReportStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var reportSummary = new ReportSummaryDto
            {
                TotalReports = summary.Sum(x => x.Count),
                Active = summary.FirstOrDefault(x => x.Status == EReportStatus.Active)?.Count ?? 0,
                InProgress = summary.FirstOrDefault(x => x.Status == EReportStatus.InProgress)?.Count ?? 0,
                Resolved = summary.FirstOrDefault(x => x.Status == EReportStatus.Resolved)?.Count ?? 0,
            };

            return reportSummary;
        }
        public async Task<List<EmergencyAlertDto>> GetEmergencyAlertsAsync()
        {
            return await _context.IssueReports
                .Include(r => r.IssueCategory)
                .Where(r => r.IssueCategory.NameEN == "Ambulance" ||
                            r.IssueCategory.NameEN == "Fire" ||
                            r.IssueCategory.NameEN == "Police")
                .GroupBy(r => r.IssueCategory.NameEN)
                .Select(g => new EmergencyAlertDto
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
       
        public async Task<List<FeedBackDto>> GetRecentFeedbackAsync()
        {
            return await _context.FeedBacks
                .Include(f => f.MobileUser) 
                .OrderByDescending(f => f.Date)
                .Take(10)
                .Select(f => new FeedBackDto
                {
                    Id = f.Id,
                    Comment = f.Comment,
                    RateValue = f.RateValue,
                    Date = f.Date,
                    MobileUserId = f.MobileUserId,
                    MobileUserName = f.MobileUser.FullName,
                    MobileUserPhone = f.MobileUser.PhoneNumber 
                })
                .ToListAsync();
        }


        public async Task<List<SocialMediaReportDto>> GetSocialMediaReports(DateTime? from, DateTime? to, string keyword, string language = "ar")
        {
            var query = _context.IssueReports
                .OfType<SocialMediaReport>() 
                .Include(r => r.IssueCategory)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(r => r.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(r => r.CreatedAt <= to.Value);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(r => r.Description.Contains(keyword) || r.Content.Contains(keyword));

            return await query
                .Select(r => new SocialMediaReportDto
                {
                    ReportId = r.Id.ToString(),
                    Description = r.Description,
                    PhotoUrl = r.ImageUrl,
                    IssueCategory = r.IssueCategory != null
                ? (language == "en" ? r.IssueCategory.NameEN : r.IssueCategory.NameAR) : null,

                    PostedAt = r.CreatedAt,
                    Likes = r.Likes,
                    Shares = r.Shares
                })
                .ToListAsync();
        }


        public async Task<List<MonthlyReportCountDto>> GetMonthlyReportCountsAsync()
        {
        var result = await _context.IssueReports
            .Where(r => !r.IsDeleted)
            .GroupBy(r => new { r.DateIssued.Year, r.DateIssued.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                ReportCount = g.Count()
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        var culture = new CultureInfo("ar-EG"); 

        var finalResult = result.Select(x => new MonthlyReportCountDto
        {
            MonthName = culture.DateTimeFormat.GetMonthName(x.Month), 
            ReportCount = x.ReportCount
        }).ToList();

        return finalResult;
        }

        public async Task<CriticalReportsDto> GetCriticalReportsDashboardAsync()
        {
            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            var today = now.Date;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var yearStart = new DateTime(today.Year, 1, 1);

            var reports = await _context.IssueReports
                .Where(r => !r.IsDeleted)
                .Select(r => new
                {
                    r.Id,
                    r.DateIssued
                })
                .ToListAsync();

            //   (Status Changes)
            var statusHistories = await _context.ReportStatusHistories
                .Where(h => !h.IsDeleted)
                .ToListAsync();

            //   وقت الاستجابة   (  InProgress أو Resolved )
            var responseTimes = reports.Select(r =>
            {
                var firstResponse = statusHistories
                    .Where(h => h.IssueReportId == r.Id &&
                                (h.ReportStatus == EReportStatus.InProgress || h.ReportStatus == EReportStatus.Resolved))
                    .OrderBy(h => h.CreatedAt)
                    .FirstOrDefault();

                if (firstResponse != null)
                {
                    return (firstResponse.CreatedAt - r.DateIssued).TotalMinutes;
                }
                else
                {
                    return (double?)null;
                }
            }).Where(x => x.HasValue).Select(x => x.Value).ToList();

            double avgResponseTime = responseTimes.Count > 0 ? responseTimes.Average() : 0;

            //  total critical reports now
            int todayReports = reports.Count(r => r.DateIssued.Date == today);
            int monthReports = reports.Count(r => r.DateIssued >= monthStart);
            int yearReports = reports.Count(r => r.DateIssued >= yearStart);

            //  total critical reports past
            var yesterday = today.AddDays(-1);
            var lastMonthStart = monthStart.AddMonths(-1);
            var lastYearStart = yearStart.AddYears(-1);

            int yesterdayReports = reports.Count(r => r.DateIssued.Date == yesterday);
            int lastMonthReports = reports.Count(r => r.DateIssued >= lastMonthStart && r.DateIssued < monthStart);
            int lastYearReports = reports.Count(r => r.DateIssued >= lastYearStart && r.DateIssued < yearStart);

            //  معدل الانخفاض
            double dailyChange = yesterdayReports == 0 ? 100 : ((double)(todayReports - yesterdayReports) / yesterdayReports) * 100;
            double monthlyChange = lastMonthReports == 0 ? 100 : ((double)(monthReports - lastMonthReports) / lastMonthReports) * 100;
            double yearlyChange = lastYearReports == 0 ? 100 : ((double)(yearReports - lastYearReports) / lastYearReports) * 100;

            return new CriticalReportsDto
            {
                AverageResponseTimeMinutes = Math.Round(avgResponseTime, 2), 
                AverageResponseTimeFormatted = $"{Math.Round(avgResponseTime)} دقيقة", 
                TodayReports = todayReports,
                MonthReports = monthReports,
                YearReports = yearReports,
                DailyChangePercentage = Math.Round(dailyChange, 2),
                MonthlyChangePercentage = Math.Round(monthlyChange, 2),
                YearlyChangePercentage = Math.Round(yearlyChange, 2)
            };
        }
        public async Task<Result> UpdateReportCategory(ChangeReportCategory changeReportCategory)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.GetUserId();
            if (userId == null)
                return Result.Failure<ReportResponse>(AuthErrors.Unauthorized);

            var report = await _context.IssueReports.FirstOrDefaultAsync(x => x.Id == changeReportCategory.ReportId);
            if (report == null)
                return Result.Failure<ReportResponse>(ReportErrors.NotFound);

            IssueCategory category = null;

            if (changeReportCategory.CategoryId.HasValue)
            {
                category = await _context.IssueCategories.FirstOrDefaultAsync(c => c.Id == changeReportCategory.CategoryId.Value);
            }
            else if (!string.IsNullOrEmpty(changeReportCategory.CategoryKey))
            {
                category = await _context.IssueCategories.FirstOrDefaultAsync(c => c.Key == changeReportCategory.CategoryKey);
            }

            if (category == null)
                return Result.Failure<ReportResponse>(ReportErrors.CategoryNotFound);

            report.IssueCategoryId = category.Id;

            await _context.SaveChangesAsync();
            return Result.Success();
        }



    }

}