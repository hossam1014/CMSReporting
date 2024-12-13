using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.Auth.Login
{
    public record LoginResponse(
        string Id,
        string UserName,
        string Email,
        string Token,
        int ExpiresIn,
        string RefreshToken
    );
}