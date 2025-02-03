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
    }
}