using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _uow.AuthRepo.Login(email, password);
            if (result == null) return BadRequest("Invalid Credentials");
            return Ok(result);
        }
    }
}