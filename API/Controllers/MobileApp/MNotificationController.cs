using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MobileApp
{
    public class MNotificationController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public MNotificationController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            var result = await _uow.MNotificationRepo.GetNotificationsAsync(userId);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
    }
}