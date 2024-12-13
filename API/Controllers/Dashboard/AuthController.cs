using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Application.Contracts.DashboardAuth.Login;
using Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Dashboard
{
    // localhost:5000/api/Auth
    public class AuthController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public AuthController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        // localhost:5000/api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _uow.AuthRepo.Login(request);

            return result.Match(
                onSuccess : () => Ok(result),
                onFailure : () => result.HandleFailure(StatusCodes.Status400BadRequest)
            );
        }
    }
}