using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers.Dashboard
{
    public class HomeController : BaseApiController
    {
        private readonly IReportRepo _reportRepo;
        public HomeController(IReportRepo reportRepo)
        {
            _reportRepo = reportRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportsNumber()
        {
            var result = await _reportRepo.NumberOfReports();

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetReportSummary()
        {
            var result = await _reportRepo.GetReportSummaryAsync();

            return result != null
                ? Ok(result)
                : BadRequest("Failed to retrieve report summary");
        }

        [HttpGet("emergencies/lastEmergencies")]
        public async Task<IActionResult> GetEmergencyAlerts()
        {
            var result = await _reportRepo.GetEmergencyAlertsAsync();
            return Ok(result);
        }

        [HttpGet("reports/top-categories")]
        public async Task<IActionResult> GetTopReportedCategories()
        {
            var result = await _reportRepo.GetTopReportedCategoriesAsync();
            return Ok(result);
        }
        [HttpGet("feedback/recent")]
        public async Task<IActionResult> GetRecentFeedback()
        {
            var result = await _reportRepo.GetRecentFeedbackAsync();
            return Ok(result);
        }

        [HttpGet("monthly-report-counts")]
        public async Task<IActionResult> GetMonthlyReportCounts()
        {
            var result = await _reportRepo.GetMonthlyReportCountsAsync();
            return Ok(result);
        }

        [HttpGet("critical-reports-dashboard")]
        public async Task<IActionResult> GetCriticalReportsDashboard()
        {
            var result = await _reportRepo.GetCriticalReportsDashboardAsync();
            return Ok(result);
        }

    }
}