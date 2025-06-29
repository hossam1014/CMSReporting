using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
   public class NotificationMessage
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public List<ChannelType> Channels { get; set; } = new();
        public List<string>? TargetUsers { get; set; }
        public NotificationCategory Category { get; set; }
    }

}
