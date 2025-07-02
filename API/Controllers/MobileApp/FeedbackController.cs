using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.MobileApp;   
using Domain.Entities;
using Application.Contracts.MobileApp.MReport;
using API.Extensions;
using Application.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IMReportRepo _mReportRepo; 

        public FeedbackController(IMReportRepo mReportRepo)
        {
            _mReportRepo = mReportRepo;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackRequest request)
        {
            if (request.RateValue < 1 || request.RateValue > 5)
                return BadRequest("Rating must be between 1 and 5.");

            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var exists = await _mReportRepo.IsUserReportExistsAsync(request.IssueReportId, userId);
            if (!exists)
                return BadRequest("Invalid report ID or this report doesn't belong to this user.");


            var feedback = new FeedBack
            {
                Comment = request.Comment,
                RateValue = request.RateValue,
                MobileUserId = userId,
                IssueReportId = request.IssueReportId,
                Date = DateTime.UtcNow
            };

            var result = await _mReportRepo.AddFeedback(feedback);

            return result.Match(
               onSuccess: () => Ok(new { message = "Feedback submitted successfully." }),
               onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
               );
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbacks = await _mReportRepo.GetAllFeedbacks();

            var feedbackDtos = feedbacks.Select(f => new FeedBackDto
            {
                Id = f.Id,
                Comment = f.Comment,
                RateValue = f.RateValue,
                Date = f.Date,
                MobileUserId = f.MobileUserId,
                MobileUserName = f.MobileUser?.FullName,    
                MobileUserPhone = f.MobileUser?.PhoneNumber,
                IssueReportId = f.IssueReportId
            }).ToList();

            return Ok(feedbackDtos);
        }

    }
}
