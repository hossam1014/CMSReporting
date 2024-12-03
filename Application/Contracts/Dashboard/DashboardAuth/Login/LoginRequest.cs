using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.DashboardAuth.Login
{
    public record LoginRequest(
        string Email, string Password
    );
}