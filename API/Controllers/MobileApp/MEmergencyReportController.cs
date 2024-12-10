using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.MobileApp.MReport;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MobileApp
{
    public class MEmergencyReportController: BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public MEmergencyReportController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(MAddEmergencyReport addReport)
        {
            var result = await _uow.MEmergencyReportRepo.AddEmergencyReportAsync(addReport);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
    }
}