using Application.Interfaces.NotificationService;
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
    public class NotificationService : INotificationService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishNotificationAsync(NotificationMessage message, string routingKey)
        {
            await _publishEndpoint.Publish(message, ctx =>
            {
                ctx.SetRoutingKey(routingKey);
            });
        }
    }
}
