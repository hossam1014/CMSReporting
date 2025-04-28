using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.Dashboard.Report;
using Application.Helpers.FilterParams;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
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

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
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

        [HttpPut]
        public async Task<IActionResult> UpdateReportStatus(ChangeReportStatus changeReportStatus)
        {
            var result = await _reportRepo.UpdateReportStatus(changeReportStatus);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpGet("socialMedia")]
        public async Task<IActionResult> GetSocialMediaReports([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string keyword)
        {
            var result = await _reportRepo.GetSharedReportsAsync(from, to, keyword);

            return Ok(new
            {
                success = true,
                reports = result
            });
        }

    }
}