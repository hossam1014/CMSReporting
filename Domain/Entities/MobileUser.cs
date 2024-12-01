using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MobileUser : BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string FCMToken { get; set; }

        public ICollection<NotificationUser> NotificationUsers { get; set; }


    }
}