using Application.DTOs;
using Application.Interfaces.Dashboard;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _roleManager.Roles
                .Select(r => new RoleDto
                {
                    Name = r.Name,
                    NameAR = r.NameAR,
                    NameEN = r.NameEN
                }).ToListAsync();
        }
    }

}
