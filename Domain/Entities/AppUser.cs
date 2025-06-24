using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public string FullName { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastSeen { get; set; }

        public string DefaultLanguage { get; set; }
        public ICollection<UserCategory> UserCategories { get; set; }

    }
}