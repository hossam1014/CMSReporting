// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using API.Extensions;
// using Application.Contracts.MobileApp.MReport;
// using Application.Interfaces;
// using Application.Interfaces.MobileApp;
// using Microsoft.AspNetCore.Mvc;

// namespace API.Controllers.MobileApp
// {
//     public class MEmergencyReportController: BaseApiController
//     {
//         private readonly IMEmergencyReportRepo _emergencyReportRepo;
//         public MEmergencyReportController(IMEmergencyReportRepo emergencyReportRepo)
//         {
//             _emergencyReportRepo = emergencyReportRepo;
//         }

//         [HttpPost]
//         public async Task<IActionResult> AddReport(MAddEmergencyReport addReport)
//         {
//             var result = await _emergencyReportRepo.AddEmergencyReportAsync(addReport);

//             return result.Match(
//                 onSuccess : () => Ok(result),
//                 onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
//             );
//         }
//     }
// }