using Application.Interfaces.NotificationService;
using Domain.Enums;
using MassTransit;
using MassTransit.Transports;
using NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NotificationServiceImpl : INotificationService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationServiceImpl(IPublishEndpoint publishEndpoint)
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
