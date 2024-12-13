using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.Auth.Register
{
    public record RegisterRequest(
        string FullName,
        string PhoneNumber,
        string Email,
        string Password
    );
}