using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
  public class TokenService : ITokenService
    {
        // private readonly IUnitOfWork _uow;

        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        // IMapper _mapper { get; }

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

        }

        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                // new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);


            // we specify here what goes inside our token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.SaudiDate().AddDays(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}