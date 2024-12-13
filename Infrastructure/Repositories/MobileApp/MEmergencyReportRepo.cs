using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.MobileApp.MReport;
using Application.Errors.MobileApp.MReport;
using Application.Interfaces.MobileApp;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.MobileApp
{
    public class MEmergencyReportRepo: IMEmergencyReportRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MEmergencyReportRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> AddEmergencyReportAsync(MAddEmergencyReport addReport)
        {
            var reportCategory = await _context.IssueCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Key == "emergency");
            if (reportCategory == null) return Result.Failure<MReportResponse>(MReportErrors.CategoryNotFound);

            var report = _mapper.Map<EmergencyReport>(addReport);
            report.IssueCategoryId = reportCategory.Id;
            report.DateIssued = DateTime.UtcNow;

            await _context.EmergencyReports.AddAsync(report);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

    }
}