using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<UserWithRolesDto>> GetUsersWithRolesAsync()
    {
        var users = await _userManager.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .ToListAsync();

        return users.Select(u => new UserWithRolesDto
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName,
            Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
        }).ToList();
    }

    public async Task<bool> CreateUserAsync(CreateUserDto dto)
    {
        var user = new AppUser
        {
            Email = dto.Email,
            UserName = dto.Email,
            FullName = dto.FullName,
            IsActive = true,
            RegisterDate = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return false;

        foreach (var role in dto.Roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);
        }

        return true;
    }

    public async Task<bool> UpdateUserAsync(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.UserName = dto.Email;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        foreach (var role in dto.Roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);
        }

        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(string id, ChangePasswordDto dto, string requesterId)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        if (id != requesterId)
            return false; 

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        return result.Succeeded;
    }
}
