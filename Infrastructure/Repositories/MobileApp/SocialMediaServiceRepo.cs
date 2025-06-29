//using Application.Abstractions;
//using Application.DTOs;
//using Application.Interfaces.MobileApp;
//using Application.Interfaces;
//using Domain.Entities;
//using Domain.Enums;
//using Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Repositories.MobileApp
//{
//    public class SocialMediaServiceRopo : ISocialMediaService
//    {
//        private readonly DataContext _context;
//        private readonly IFileRepo _fileRepo;

//        public SocialMediaServiceRopo(DataContext context, IFileRepo fileRepo)
//        {
//            _context = context;
//            _fileRepo = fileRepo;
//        }

//        public async Task<Result> ShareReportAsync(ShareReportDto dto, string userId)
//        {
//            var report = await _context.IssueReports
//                .FirstOrDefaultAsync(r => r.Id == dto.ReportId && !r.IsDeleted);

//            if (report == null)
//                return Result.Failure(new Error("NotFound", "Report not found."));

//            if (report.MobileUserId != userId)
//                return Result.Failure(new Error("Unauthorized", "You are not authorized to share this report."));

//            string imageUrl = report.ImageUrl;
//            if (dto.Image != null)
//            {
//                var path = "SharedReports";
//                imageUrl = await _fileRepo.CreateFileAsync(dto.Image, path);
//            }

//            var sharedReport = new SocialMediaReport
//            {
//                Description = report.Description,
//                ImageUrl = imageUrl,
//                IssueCategoryId = report.IssueCategoryId,
//                MobileUserId = report.MobileUserId,
//                ReportStatus = report.ReportStatus,
//                DateIssued = DateTime.UtcNow,
//                ReportType = EReportType.SocialMedia,
//                Latitude = report.Latitude,
//                Longitude = report.Longitude,
//                Address = report.Address,
//                Content = report.Description,
//                CreatedAt = DateTime.UtcNow,
//                Likes = 0,
//                Shares = 0
//            };

//            await _context.IssueReports.AddAsync(sharedReport);
//            await _context.SaveChangesAsync();

//            return Result.Success();
//        }
//    }

//}
