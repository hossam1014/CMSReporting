using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Dashboard
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();

    }
}
