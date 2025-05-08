using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Repository;
using Application.Abstractions;
using Application.Contracts.MobileApp.MReport;
using Application.Errors.Auth;
using Application.Errors.MobileApp.MReport;
using Application.Interfaces;
using Application.Interfaces.MobileApp;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories.MobileApp
{
    public class MReportRepo : BaseRepo<IssueReport>, IMReportRepo
    {
        private readonly IMapper _mapper;
        private readonly IFileRepo _fileRepo;
        // private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MReportRepo(DataContext context, IMapper mapper, IFileRepo fileRepo, IHttpContextAccessor httpContextAccessor)
            : base(context, mapper)
        {
            _mapper = mapper;
            _fileRepo = fileRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> AddReport(MAddReport addReport)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (userId == null)
                return Result.Failure<MReportResponse>(AuthErrors.UserNotFound);

            // Get or create the mobile user
            var mobileUserResult = await GetOrCreateMobileUserAsync(userId);
            if (mobileUserResult.IsFailure)
                return Result.Failure(mobileUserResult.Error);

            var reportCategory = await _context.IssueCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Key == addReport.IssueCategoryKey);

            if (reportCategory == null)
                return Result.Failure<MReportResponse>(MReportErrors.CategoryNotFound);

            string imageUrl = null;
            if (addReport.Image != null)
            {
                var path = "IssueReports";
                imageUrl = await _fileRepo.CreateFileAsync(addReport.Image, path);
            }

            var report = _mapper.Map<IssueReport>(addReport);
            report.MobileUserId = userId;
            report.IssueCategoryId = reportCategory.Id;
            report.ImageUrl = imageUrl;
            report.DateIssued = DateTime.UtcNow;

            await _context.IssueReports.AddAsync(report);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<List<MReportResponse>>> GetReportsByUserId()
        {

            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (userId == null)
                return Result.Failure<List<MReportResponse>>(AuthErrors.UserNotFound);

            var reports = await _context.IssueReports.AsNoTracking()
                                                    .Where(x => x.MobileUserId == userId)
                                                    .ProjectTo<MReportResponse>(_mapper.ConfigurationProvider)
                                                    .ToListAsync();

            return Result.Success(reports);


        }

        public async Task<Result> SubmitEmergencyReport(EmergencyReportRequest request)
        {

            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (userId == null)
                return Result.Failure<MReportResponse>(AuthErrors.UserNotFound);

            // Get or create the mobile user
            var mobileUserResult = await GetOrCreateMobileUserAsync(userId);
            if (mobileUserResult.IsFailure)
                return Result.Failure(mobileUserResult.Error);

            var serviceExists = await _context.EmergencyServices.AsNoTracking()
                                    .AnyAsync(x => x.Id == request.EmergencyServiceId);
            if (!serviceExists)
                return Result.Failure(MReportErrors.EmergencyServiceNotFound);

            var report = new EmergencyReport
            {
                EmergencyServiceId = request.EmergencyServiceId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                MobileUserId = userId,
                DateIssued = DateTime.UtcNow,
                ReportStatus = EReportStatus.Active,
                Address = request.Address,
                IssueCategoryId = await _context.IssueCategories.AsNoTracking().Where(x => x.Key == "emergency").Select(x => x.Id).FirstOrDefaultAsync(),

            };

            await _context.EmergencyReports.AddAsync(report);
            await _context.SaveChangesAsync();

            return Result.Success();

        }
        public async Task<Result> AddFeedback(FeedBack feedback)
        {
            try
            {
                await _context.FeedBacks.AddAsync(feedback);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(MReportErrors.FeedbackSaveFailed);
            }
        }

        public async Task<List<FeedBack>> GetAllFeedbacks()
        {
            return await _context.FeedBacks
                                .Include(f => f.MobileUser)
                                .AsNoTracking()
                                .OrderByDescending(f => f.Date)
                                .ToListAsync();
        }

        private async Task<Result> GetOrCreateMobileUserAsync(string userId)
        {


            var existingUser = await _context.MobileUsers.FirstOrDefaultAsync(x => x.Id == userId);
            if (existingUser != null)
                return Result.Success();

            var newUser = new MobileUser
            {
                Id = userId,
                FullName = _httpContextAccessor.HttpContext?.User.GetUsername(),
                Email = _httpContextAccessor.HttpContext?.User.GetEmail(),
                PhoneNumber = new Random().Next(100000000, 999999999).ToString(), // Placeholder for phone number
                CreatedAt = DateTime.UtcNow
                // Add other default properties if needed
            };

            _context.MobileUsers.Add(newUser);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

    }

}