using System;
using System.Runtime;
using System.Threading.Tasks;
using Application.Abstractions; 
using Application.DTOs;
using Application.Errors.SocialMedia;
using Application.Interfaces.Dashboard;
using Application.Interfaces.SocialMedia;
using Application.Options;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.SocialMedia
{

    public class SocialMediaReportService : ISocialMediaReportService
    {
        private readonly DataContext _context;
        private readonly ISocialMediaService _socialMediaService;
        private readonly IConfiguration _config;
        private readonly IReportRepo _reportRepo;
        private readonly SocialMediaSettings _settings;


        public SocialMediaReportService(
            DataContext context,
            ISocialMediaService socialMediaService,
            IConfiguration config,
            IReportRepo reportRepo,
            IOptions<SocialMediaSettings> settings)
        {
            _context = context;
            _socialMediaService = socialMediaService;
            _config = config;
            _reportRepo = reportRepo;
            _settings = settings.Value;

        }

        public async Task<Result> ShareReportAsync(ShareReportRequest request, string userId, string token)
        {
            // 1. نجيب البلاغ
            var report = await _reportRepo.GetIssueReportEntityById(request.ReportId);
            if (report == null)
                return Result.Failure(SocialMediaErrors.ReportNotFound);

            // 2. نحدد الـ endpoint بناءً على نوع المشاركة
            string endpoint = request.Platform == "AdminPost"
                ? _settings.AdminPostUrl
                : _settings.UserPostUrl;

            // 3. نستخدم HttpClient 
            var shareSuccess = await _socialMediaService.ShareToPlatform(
                endpoint,
                token,
                caption: report.Description,
                tag: report.IssueCategory?.NameEN ?? "CityReport",
                mediaUrl: report.ImageUrl
            );

            if (!shareSuccess)
                return Result.Failure(SocialMediaErrors.ShareFailed);

            // 4. نحفظ البلاغ في جدول SocialMediaReports لو مش متسجل قبل كده
            var alreadyShared = await _context.IssueReports
                .OfType<SocialMediaReport>()
                .AnyAsync(r => r.Id == report.Id);

             if (!alreadyShared)
            {
                var socialReport = new SocialMediaReport
                {
                    //Id = report.Id,
                    Description = report.Description,
                    ImageUrl = report.ImageUrl,
                    IssueCategoryId = report.IssueCategoryId,
                    MobileUserId = report.MobileUserId,
                    ReportStatus = report.ReportStatus,
                    DateIssued = report.DateIssued,
                    ReportType = report.ReportType,
                    Latitude = report.Latitude,
                    Longitude = report.Longitude,
                    Address = report.Address,
                    CreatedAt = DateTime.UtcNow,
                    Content = report.Description,
                    Likes = 0,
                    Shares = 1,
                    CommentsCount = 0
                };

               // _context.Entry(report).State = EntityState.Detached;  
                _context.IssueReports.Add(socialReport);
            }
            else
            {
                // لو متسجل بالفعل، نزود عدد shares
                var existing = await _context.IssueReports
                    .OfType<SocialMediaReport>()
                    .FirstOrDefaultAsync(r => r.Id == report.Id);

                existing.Shares += 1;
            }

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}

