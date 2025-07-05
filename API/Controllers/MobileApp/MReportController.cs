using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.MobileApp.MReport;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.MobileApp;
using Application.Interfaces.SocialMedia;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MobileApp
{
    public class MReportController : BaseApiController
    {
        private readonly IMReportRepo _reportRepo;
        private readonly ISocialMediaReportService _socialMediaReportService;

        public MReportController(IMReportRepo reportRepo , ISocialMediaReportService socialMediaReportService)
        {
            _reportRepo = reportRepo;
            _socialMediaReportService = socialMediaReportService;

        }

        [HttpPost]
        public async Task<IActionResult> AddReport([FromForm] MAddReport addReport)
        {
            var result = await _reportRepo.AddReport(addReport);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpPost("update-location")]
        public async Task<IActionResult> UpdateLocation(UpdateLocationRequest request)
        {
            var userId = User.FindFirst("uid")?.Value; 
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _reportRepo.UpdateLocationAsync(userId, request);

            return result.Match(
                onSuccess: () => Ok(result),
                onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
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



        [HttpGet("my-reports")]
        public async Task<IActionResult> GetReports()
        {
            var result = await _reportRepo.GetReportsByUserId();

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
        [HttpPost("social-media/share")]
        [Authorize]
        public async Task<IActionResult> ShareReportByUser([FromBody] ShareReportRequest request)
        {
            var userId = User.GetUserId();
            var userToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var result = await _socialMediaReportService.ShareReportAsync(request, false, userId, userToken);

            return result.Match(
                onSuccess: () => Ok(new { message = "Report shared successfully on social media." }),
                onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }



    }
}