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
        Task<List<UserWithRolesDto>> GetUsersWithRolesAsync();
        Task<bool> CreateUserAsync(CreateUserDto dto);
        Task<bool> UpdateUserAsync(string id, UpdateUserDto dto);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto);
    }
}
