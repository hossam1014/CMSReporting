using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public VerificationService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SendOTP(string PhoneNumber, string Code)
        {

            // var Number = "+966" + number;
            // var user = await _userManager.Users.FirstOrDefaultAsync(x =>
            //     x.PhoneNumber == PhoneNumber);

            // if (user == null) return;

            var baseAddress = new Uri(_configuration["MsgatUrl"]);
            
            var messege = "Pin Code is: " + Code;

            using var httpClient = new HttpClient { BaseAddress = baseAddress };
            {
                Dictionary<string, string> jsonValues = new Dictionary<string, string>
                {
                    { "userName", "Easacc@2023" },
                    { "numbers", PhoneNumber },
                    { "userSender", "Easacc" },
                    { "apiKey", "58cd7dfe6c8ab2e04acb009d00590b8a" },
                    { "msg", messege }
                };
                using (var content = new StringContent(JsonSerializer.Serialize(jsonValues), System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("/gw/sendsms.php", content))
                    {
                        string responseHeaders = response.Headers.ToString();

                        string responseData = await response.Content.ReadAsStringAsync();

                        
                    }
                }
            }

        }
    }
}