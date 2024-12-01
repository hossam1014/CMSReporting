using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class NotificationUser
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        public int UserId { get; set; }
        public MobileUser User { get; set; }
    }
}