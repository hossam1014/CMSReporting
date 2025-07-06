using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
namespace Application.Interfaces.Dashboard
{
    public interface IUserService
    {
        Task<List<UserWithRolesDto>> GetUsersWithRolesAsync(bool onlyMobile = false);
        Task<bool> CreateUserAsync(CreateUserDto dto);
        Task<bool> UpdateUserAsync(string id, UpdateUserDto dto);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task<UserProfileDto> GetProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto);
        Task<bool> ResetPasswordAsync(string userId, string newPassword);
        Task<bool> DeleteUserAsync(string userId);
    }
}
