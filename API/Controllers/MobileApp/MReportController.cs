using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.MobileApp.MReport;
using Application.Interfaces;
using Application.Interfaces.MobileApp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MobileApp
{
    public class MReportController : BaseApiController
    {
        private readonly IMReportRepo _reportRepo;
        public MReportController(IMReportRepo reportRepo)
        {
            _reportRepo = reportRepo;
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(MAddReport addReport)
        {
            var result = await _reportRepo.AddReport(addReport);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetReports(string userId)
        {
            var result = await _reportRepo.GetReportsByUserId(userId);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
    }
}