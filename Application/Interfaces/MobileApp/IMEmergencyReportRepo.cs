using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.MobileApp.MReport;
using Application.DTOs;
using Application.Helpers;
using Application.Helpers.FilterParams;

namespace Application.Interfaces.MobileApp
{
    public interface IMEmergencyReportRepo
    {
        public Task<Result> AddEmergencyReportAsync(MAddEmergencyReport addReport);
        Task<Result<PagedList<EmergencyReportDto>>> GetEmergencyReportsAsync(
            EmergencyQueryParams queryParams, string language = "ar");

    }
}