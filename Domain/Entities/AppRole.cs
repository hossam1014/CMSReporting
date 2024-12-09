using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppRole: IdentityRole
    {
        [MaxLength(100)]
        public string NameAR { get; set; }

        [MaxLength(100)]
        public string NameEN { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}