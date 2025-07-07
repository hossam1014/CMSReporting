using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.Auth.Login;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Dashboard
{
    // localhost:5000/api/Auth
    public class AuthController : BaseApiController
    {
        private readonly IAuthRepo _authRepo;
        public AuthController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }


       // localhost:5000/api/Auth/login
       [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authRepo.Login(request);

            return result.Match(
                onSuccess: () => Ok(result),
                onFailure: () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }


    }
}