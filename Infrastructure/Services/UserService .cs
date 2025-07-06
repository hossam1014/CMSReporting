using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly DataContext _context;

    public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, DataContext context)

    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;

    }

    public async Task<List<UserWithRolesDto>> GetUsersWithRolesAsync(bool onlyMobile = false)
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
           PhoneNumber = u.PhoneNumber,
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

        if (dto.CategoryIds != null && dto.CategoryIds.Any())
        {
            foreach (var catId in dto.CategoryIds)
            {
                _context.UserCategories.Add(new UserCategory
                {
                    UserId = user.Id,
                    CategoryId = catId
                });
            }

            await _context.SaveChangesAsync();
        }

       

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
        //user.Email = dto.Email;  
        //user.UserName = dto.Email;  


        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        foreach (var role in dto.Roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);
        }

        var existingCategories = _context.UserCategories.Where(uc => uc.UserId == user.Id);
        _context.UserCategories.RemoveRange(existingCategories);

        if (dto.CategoryIds != null && dto.CategoryIds.Any())
        {
            foreach (var catId in dto.CategoryIds)
            {
                _context.UserCategories.Add(new UserCategory
                {
                    UserId = user.Id,
                    CategoryId = catId
                });
            }
        }

        await _userManager.UpdateAsync(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        return result.Succeeded;
    }

    public async Task<UserProfileDto> GetProfileAsync(string userId)
    {
        var user = await _userManager.Users
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;

        return new UserProfileDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        };
    }
    public async Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.FullName = $"{dto.FirstName} {dto.LastName}".Trim();
        user.PhoneNumber = dto.PhoneNumber;

        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        return result.Succeeded;
    }
    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);

        return true;
    }

}
