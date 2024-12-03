using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.DashboardAuth.Register
{
    public record RegisterRequest(
        string FullName,
        string PhoneNumber,
        string Email,
        string Password
    );
}