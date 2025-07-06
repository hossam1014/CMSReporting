using Application.DTOs;
using Application.Interfaces.Dashboard;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Dashboard
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(IUserService userService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> GetUsers([FromQuery] bool onlyMobile = false)
        {
            var users = await _userService.GetUsersWithRolesAsync(onlyMobile);
            return Ok(users);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            if (!result) return BadRequest("Error creating user");
            return Ok("User created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return BadRequest("Error updating user");
            return Ok("User updated successfully");
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {

            var currentUserId = _userManager.GetUserId(User);
            var result = await _userService.ChangePasswordAsync(currentUserId, dto);
            if (!result) return BadRequest("Error changing password");
            return Ok("Password changed successfully");
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var currentUserId = _userManager.GetUserId(User);
            var profile = await _userService.GetProfileAsync(currentUserId);

            if (profile == null)
                return NotFound("User not found.");

            return Ok(profile);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _userService.UpdateProfileAsync(userId, dto);

            if (!result)
                return BadRequest("Failed to update profile");

            return Ok("Profile updated successfully");
        }

        [HttpPut("{id}/reset-password")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordDto dto)
        {
            var result = await _userService.ResetPasswordAsync(id, dto.NewPassword);
            if (!result) return BadRequest("Failed to reset password.");

            return Ok("Password reset successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return NotFound("User not found or could not be deleted.");

            return Ok("User deleted successfully.");
        }

    }
}
