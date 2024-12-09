using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MobileUser
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }


        public ICollection<NotificationUser> NotificationUsers { get; set; }

        public ICollection<FeedBack> FeedBacks { get; set; }


    }
}