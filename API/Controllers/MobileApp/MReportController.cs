using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.MobileApp.MReport;
using Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MobileApp
{
    public class MReportController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public MReportController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(MAddReport addReport)
        {
            var result = await _uow.MReportRepo.AddReport(addReport);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetReports(string userId)
        {
            var result = await _uow.MReportRepo.GetReportsByUserId(userId);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
    }
}