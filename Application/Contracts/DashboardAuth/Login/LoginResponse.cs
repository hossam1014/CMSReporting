using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.DashboardAuth.Login
{
    public record LoginResponse(
        int Id,
        string UserName,
        string Email,
        string Token,
        int ExpiresIn,
        string RefreshToken
    );
}