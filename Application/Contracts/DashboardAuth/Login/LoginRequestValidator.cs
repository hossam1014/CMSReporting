using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Contracts.DashboardAuth.Login
{
    public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);

        }
    }
}