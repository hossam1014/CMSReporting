using API.Extensions;
using Application.DTOs;
using Application.Interfaces.NotificationService;
using Application.Interfaces.SocialMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;

namespace API.Controllers.Dashboard
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {
        private readonly ISocialMediaReportService _socialMediaReportService;
        private readonly INotificationService _notificationService;

        public AdminController(ISocialMediaReportService socialMediaReportService, INotificationService notificationService)
        {
            _socialMediaReportService = socialMediaReportService;
            _notificationService = notificationService;

        }

        [HttpPost("share")]
        public async Task<IActionResult> ShareReportByAdmin([FromBody] ShareReportRequest request)
        {
            var result = await _socialMediaReportService.ShareReportAsync(request, true);

            return result.Match(
                onSuccess: () => Ok(new { message = "Admin post shared successfully on social media." }),
                onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            var message = new NotificationMessage
            {
                Title = request.Title,
                Body = request.Body,
                Channels = request.Channels,
                Type = NotificationType.SystemWide,  
                Category = NotificationCategory.Alert 
            };

            await _notificationService.PublishNotificationAsync(message, "user.notification.created");
            return Ok("Notification sent.");
        }

    }
}
