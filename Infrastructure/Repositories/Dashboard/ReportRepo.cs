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
            var reportsQuery = _context.IssueReports.AsNoTracking()
                                                .Where(x => !x.IsDeleted)
                                                .OrderByDescending(x => x.DateIssued)
                                                .ProjectTo<ReportResponse>(_mapper.ConfigurationProvider);

            var result = await PagedList<ReportResponse>.CreateAsync(reportsQuery, reportParams);

            return Result.Success(result);

        }

        public async Task<Result<ReportResponse>> GetReportById(int id)
        {
            var report = await _context.IssueReports.Where(x => !x.IsDeleted && x.Id == id)
                                                    .ProjectTo<ReportResponse>(_mapper.ConfigurationProvider)
                                                    .FirstOrDefaultAsync();

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
                UserId = userId
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
        public async Task<List<TopCategoryDto>> GetTopReportedCategoriesAsync()
        {
            return await _context.IssueReports
                .Include(r => r.IssueCategory)
                .Where(r => !r.IsDeleted)
                .GroupBy(r => r.IssueCategory.NameEN)
                .OrderByDescending(g => g.Count()) 
                .Take(5) 
                .Select(g => new TopCategoryDto
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
        public async Task<List<FeedBackDto>> GetRecentFeedbackAsync()
        {
            return await _context.FeedBacks
                .OrderByDescending(f => f.Date) 
                .Take(10) 
                .Select(f => new FeedBackDto
                {
                    Comment = f.Comment,
                    RateValue = f.RateValue,
                    Date = f.Date,
                    MobileUserName = f.MobileUser!= null ? f.MobileUser.FullName : "Unknown" 
                })
                .ToListAsync();
        }
    }
}