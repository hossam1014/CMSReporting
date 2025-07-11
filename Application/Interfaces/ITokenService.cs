using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task<(string token, int expiresIn)> CreateToken(AppUser user);
    }
}