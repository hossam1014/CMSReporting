using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.MobileApp.MReport;
using Application.DTOs;
using Application.Errors.MobileApp.MReport;
using Application.Helpers;
using Application.Helpers.FilterParams;
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

            await _context.IssueReports.AddAsync(report); // ////
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<PagedList<EmergencyReportDto>>> GetEmergencyReportsAsync(EmergencyQueryParams queryParams, string language = "ar")
        {
            var query = _context.IssueReports /////
                .OfType<EmergencyReport>()  // / // 
                .Include(r => r.EmergencyService)
                .Include(r => r.IssueCategory)
                .Include(r => r.MobileUser)
                .Where(r =>
                    (string.IsNullOrEmpty(queryParams.Keyword) ||
                     r.Description.ToLower().Contains(queryParams.Keyword.ToLower()) ||
                     (r.MobileUser.FullName != null && r.MobileUser.FullName.ToLower().Contains(queryParams.Keyword.ToLower())) ||
                     (r.MobileUser.PhoneNumber != null && r.MobileUser.PhoneNumber.ToLower().Contains(queryParams.Keyword.ToLower()))
                    ) &&
                    (string.IsNullOrEmpty(queryParams.EmergencyType) ||
                     r.EmergencyService.NameEN.ToLower() == queryParams.EmergencyType.ToLower() ||
                     r.EmergencyService.NameAR == queryParams.EmergencyType
                    ) &&
                    (string.IsNullOrEmpty(queryParams.Status) ||
                     r.ReportStatus.ToString().ToLower() == queryParams.Status.ToLower()
                    ) &&
                    (queryParams.From == null || r.DateIssued >= queryParams.From) &&
                    (queryParams.To == null || r.DateIssued <= queryParams.To)
                )
                .AsNoTracking()
                .AsQueryable();

            var projected = query.OrderByDescending(r => r.DateIssued)    // ////// 
                .Select(r => new EmergencyReportDto
                {
                    Id = r.Id,
                    //Description = r.Description,
                    EmergencyType = language == "en" ? r.EmergencyService.NameEN : r.EmergencyService.NameAR,
                    //IssueCategory = language == "en" ? r.IssueCategory.NameEN : r.IssueCategory.NameAR,
                    DateIssued = r.DateIssued,
                    Status = r.ReportStatus.ToString(),
                    UserFullName = r.MobileUser.FullName,
                    UserPhoneNumber = r.MobileUser.PhoneNumber,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude,
                    Address = r.Address
                });

            var pagedList = await PagedList<EmergencyReportDto>.CreateAsync(projected, queryParams);

            return Result.Success(pagedList);
        }

    }
}