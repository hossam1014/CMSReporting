using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.DashboardAuth.Login;
using Application.Errors.Auth;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories.Dashboard
{
    public class AuthRepo : IAuthRepo
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AuthRepo(ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }
        public async Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) return Result.Failure<LoginResponse>(AuthErrors.UserNotFound);

            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!checkPasswordResult) return Result.Failure<LoginResponse>(AuthErrors.InvalidCredentials);

            // if (!user.PhoneNumberConfirmed)
            // {
            //     return await SendCode(user.PhoneNumber);
            // }

            var result = new LoginResponse(
                    user.Id,
                    user.UserName,
                    user.Email,
                    (await _tokenService.CreateToken(user)).token,
                    (await _tokenService.CreateToken(user)).expiresIn,
                    null
                    );

            return Result.Success(result);
        }
    }
}