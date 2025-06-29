using Domain.Enums;
using Domain.Events;
using MassTransit;
using MassTransit.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NotificationService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishEmergencyAlertAsync(string title, string body, List<string> userIds)
        {
            var message = new NotificationMessage
            {
                Title = title,
                Body = body,
                Type = NotificationType.Group,
                Channels = new List<ChannelType> { ChannelType.Push, ChannelType.Email },
                TargetUsers = userIds,
                Category = NotificationCategory.Alert
            };
            await _publishEndpoint.Publish(message, ctx =>
        {
            ctx.SetRoutingKey("user.notification.created");
        });
        }
    }
}
