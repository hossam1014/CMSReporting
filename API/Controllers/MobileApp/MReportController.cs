using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.MobileApp.MReport;
using Application.Interfaces;
using Application.Interfaces.MobileApp;
using Domain.Entities;
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

        [HttpPost("emergency")]
        public async Task<IActionResult> SubmitEmergencyReport([FromBody] EmergencyReportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportRepo.SubmitEmergencyReport(request);

            return result.Match( 
                onSuccess: () => Ok(new { message = "Emergency report submitted successfully!" }),
                onFailure: () => result.HandleFailure(StatusCodes.Status500InternalServerError)
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