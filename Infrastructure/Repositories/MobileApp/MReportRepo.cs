using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories.MobileApp
{
    public class MReportRepo : BaseRepo<IssueReport>, IMReportRepo
    {
        private readonly IMapper _mapper;
        public MReportRepo(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _mapper = mapper;
        }

        public async Task<Result> AddReport(MAddReport addReport)
        {
            var reportCategory = await _context.IssueCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Key == addReport.IssueCategoryKey);
            if (reportCategory == null) return Result.Failure<MReportResponse>(MReportErrors.CategoryNotFound);

            var isUserExist = await _context.MobileUsers.AnyAsync(x => x.Id == addReport.MobileUserId);
            if (!isUserExist) return Result.Failure(AuthErrors.UserNotFound);

            // handle the image
            string imageUrl = null;

            var report = _mapper.Map<IssueReport>(addReport);
            report.IssueCategoryId = reportCategory.Id;
            report.ImageUrl = imageUrl;

            await _context.IssueReports.AddAsync(report);
            await _context.SaveChangesAsync();

            return Result.Success();


        }

        public async Task<Result<List<MReportResponse>>> GetReportsByUserId(string userId)
        {
            var reports = await _context.IssueReports.AsNoTracking()
                                                    .Where(x => x.MobileUserId == userId)
                                                    .ProjectTo<MReportResponse>(_mapper.ConfigurationProvider)
                                                    .ToListAsync();

            return Result.Success(reports);


        }

        public async Task<Result> SubmitEmergencyReport(EmergencyReportRequest request)
        {
            try
            {
                var serviceExists = await _context.EmergencyServices.AsNoTracking()
                                        .AnyAsync(x => x.Id == request.EmergencyServiceId);
                if (!serviceExists)
                    return Result.Failure(MReportErrors.EmergencyServiceNotFound);

                var report = new EmergencyReport
                {
                    EmergencyServiceId = request.EmergencyServiceId,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Description = request.Description,
                };

                await _context.EmergencyReports.AddAsync(report);
                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(MReportErrors.ReportSaveFailed);
            }
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
                                .AsNoTracking()
                                .OrderByDescending(f => f.Date)
                                .ToListAsync();
        }

    }

}