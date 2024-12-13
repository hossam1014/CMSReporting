using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.Dashboard.Report;
using Application.Helpers.FilterParams;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Dashboard
{
    public class ReportController: BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public ReportController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetReports([FromQuery] BaseParams reportParams)
        {
            var result = await _uow.ReportRepo.GetAllReports(reportParams);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var result = await _uow.ReportRepo.GetReportById(id);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReportStatus(ChangeReportStatus changeReportStatus)
        {
            var result = await _uow.ReportRepo.UpdateReportStatus(changeReportStatus);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
    }
}