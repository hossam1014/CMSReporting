using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.Dashboard.Report;
using Application.DTOs;
using Application.Helpers;
using Application.Helpers.FilterParams;
using Domain.Enums;

namespace Application.Interfaces.Dashboard
{
    public interface IReportRepo
    {
        Task<Result<PagedList<ReportResponse>>> GetAllReports(BaseParams reportParams);
        Task<Result<ReportResponse>> GetReportById(int id);
        Task<Result> UpdateReportStatus(ChangeReportStatus changeReportStatus);
        Task<Result<int>> NumberOfReports();
        Task<ReportSummaryDto> GetReportSummaryAsync();
        Task<List<EmergencyAlertDto>> GetEmergencyAlertsAsync();
        Task<List<TopCategoryDto>> GetTopReportedCategoriesAsync();
        Task<List<FeedBackDto>> GetRecentFeedbackAsync();

    }
}