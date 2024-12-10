using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Dashboard
{
    public interface IAuthRepo
    {
        public Task<string> Login(string email, string password);
    }
}