using API.Extensions;
using Application.DTOs;
using Application.Interfaces.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MobileApp
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : BaseApiController
    {
        private readonly ISocialMediaService _socialMediaService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SocialMediaController(ISocialMediaService socialMediaService, IHttpContextAccessor httpContextAccessor)
        {
            _socialMediaService = socialMediaService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("share")]
        public async Task<IActionResult> ShareReport([FromForm] ShareReportDto dto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.GetUserId();
            if (userId == null)
                return Unauthorized("User not authenticated.");

            var result = await _socialMediaService.ShareReportAsync(dto, userId);

            return result.Match(
                onSuccess: () => Ok(new { message = "تمت مشاركة البلاغ على السوشيال ميديا بنجاح" }),
                onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
    }
}
