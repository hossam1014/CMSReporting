using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.Dashboard.Report;
using Application.Helpers.FilterParams;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Dashboard
{
    public class ReportController: BaseApiController
    {
        private readonly IReportRepo _reportRepo;
        public ReportController(IReportRepo reportRepo)
        {
            _reportRepo = reportRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetReports([FromQuery] BaseParams reportParams)
        {
            var result = await _reportRepo.GetAllReports(reportParams);

            if (!result.IsSuccess)
                return result.HandleFailure(StatusCodes.Status400BadRequest);

            Response.AddPaginationHeader(
                result.Value.CurrentPage,
                result.Value.PageSize,
                result.Value.TotalCount,
                result.Value.TotalPages
            );

            //return result.Match(
            //    onSuccess: () => Ok(result),
            //    onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
            //);
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var result = await _reportRepo.GetReportById(id);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpPut("update-ReportStatus")]
        [Authorize(Policy = "DashboardPolicy")]

        public async Task<IActionResult> UpdateReportStatus(ChangeReportStatus changeReportStatus)
        {
            var result = await _reportRepo.UpdateReportStatus(changeReportStatus);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpGet(template: "social-media-reports")]
        [Authorize]
        public async Task<IActionResult> GetSocialReports(
                DateTime? from,
                DateTime? to,
                string keyword,
                string language = "ar")
        {
            var reports = await _reportRepo.GetSocialMediaReports(from, to, keyword, language);
            return Ok(reports);
        }

        [HttpPut("update-category")]
        [Authorize(Policy = "DashboardPolicy")]
        public async Task<IActionResult> UpdateReportCategory(ChangeReportCategory changeReportCategory)
        {
            var result = await _reportRepo.UpdateReportCategory(changeReportCategory);

            return result.Match(
                onSuccess: () => Ok(new { message = "Category updated successfully" }),
                onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }


    }
}