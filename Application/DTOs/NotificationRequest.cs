using NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class NotificationRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<ChannelType> Channels { get; set; } = new();
    }
}
