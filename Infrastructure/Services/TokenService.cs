using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly JwtConfigurations _jwtConfigurations;



        public TokenService(UserManager<AppUser> userManager, IOptions<JwtConfigurations> options)
        {
            _userManager = userManager;
            // _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _jwtConfigurations = options.Value;

          
        }

        public async Task<(string token, int expiresIn)> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                // new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            Console.WriteLine($"Creating token for user: {user.UserName}, ID: {user.Id}");

            var roles = await _userManager.GetRolesAsync(user);
            Console.WriteLine($"Roles: {string.Join(",", roles)}");


            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.Secret));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            // we specify here what goes inside our token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtConfigurations.ValidIssuer,
                Audience = _jwtConfigurations.ValidAudience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_jwtConfigurations.TokenExpiryTimeInMinutes),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), _jwtConfigurations.TokenExpiryTimeInMinutes * 60);
        }


    }
}