using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Repository;
using Application.Abstractions;
using Application.Contracts.MobileApp.MReport;
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
        public MReportRepo(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<Result> AddReport(MAddReport addReport)
        {
            var reportCategory = await _context.IssueCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Key == addReport.IssueCategoryKey);
            if (reportCategory == null) return Result.Failure<MReportResponse>(MReportErrors.CategoryNotFound);

            // handle the image
            var imageUrl = string.Empty;

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
    }
}