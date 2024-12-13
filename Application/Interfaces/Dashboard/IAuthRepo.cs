using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.Auth.Login;

namespace Application.Interfaces.Dashboard
{
    public interface IAuthRepo
    {
        Task<Result<LoginResponse>> Login(LoginRequest request);
    }
}