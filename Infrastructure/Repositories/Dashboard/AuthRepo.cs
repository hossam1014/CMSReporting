using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AuthRepo(DataContext dataContext, ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _context = dataContext;
            _tokenService = tokenService;
            _userManager = userManager;
        }
        public async Task<string> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (result == false)
            {
                return null;
            }

            var tokenResult = await _tokenService.CreateToken(user);

            return tokenResult.token;
        }
    }
}