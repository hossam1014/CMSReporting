//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using API.Extensions;
//using Application.Interfaces;
//using Application.Interfaces.MobileApp;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers.MobileApp
//{
//    public class MNotificationController : BaseApiController
//    {
//        private readonly IMNotificationRepo _notificationRepo;
//        public MNotificationController(IMNotificationRepo notificationRepo)
//        {
//            _notificationRepo = notificationRepo;
//        }

//        [HttpGet("{userId}")]
//        public async Task<IActionResult> GetNotifications(string userId)
//        {
//            var result = await _notificationRepo.GetNotificationsAsync(userId);

//            return result.Match(
//                onSuccess : () => Ok(result),
//                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
//            );
//        }
//    }
//}