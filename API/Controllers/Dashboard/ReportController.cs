using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.Dashboard.Report;
using Application.DTOs;
using Application.Helpers.FilterParams;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Application.Interfaces.SocialMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Dashboard
{
    public class ReportController: BaseApiController
    {
        private readonly IReportRepo _reportRepo;
        private readonly ISocialMediaReportService _socialMediaReportService;

        public ReportController(IReportRepo reportRepo, ISocialMediaReportService socialMediaReportService)
        {
            _reportRepo = reportRepo;
            _socialMediaReportService = socialMediaReportService;

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
        [HttpGet("social-media-reports")]
        [Authorize]
        public async Task<IActionResult> GetSocialReports([FromQuery] BaseParams reportParams, string language = "ar")
        {
            var result = await _reportRepo.GetSocialMediaReports(reportParams, language);

            if (!result.IsSuccess)
                return result.HandleFailure(StatusCodes.Status400BadRequest);

            Response.AddPaginationHeader(
                result.Value.CurrentPage,
                result.Value.PageSize,
                result.Value.TotalCount,
                result.Value.TotalPages
            );

            return Ok(result);
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
        [HttpPost("social-media/share")]
        [Authorize]
        public async Task<IActionResult> ShareReport([FromBody] ShareReportRequest request)
        {
            // نستخرج التوكن من الهيدر (في حالة UserPost)
            var userToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // نستخرج UserId من التوكن
            var userId = User.GetUserId(); 

            var result = await _socialMediaReportService.ShareReportAsync(request, userId, userToken);

            if (!result.IsSuccess)
                return result.HandleFailure(StatusCodes.Status400BadRequest);

            return Ok(new { message = "Report has been shared successfully." });
        }


    }
}