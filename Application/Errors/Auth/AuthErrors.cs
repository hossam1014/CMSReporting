using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Application.Errors.Auth
{
    public static class AuthErrors
    {
        public static readonly Error UserNotFound = new("User.NotFound", "Can't Find This User");
        public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid Email Or Password");
    }
}