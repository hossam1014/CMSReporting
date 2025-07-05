using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.NotificationService
{
    public interface INotificationService
    {
        Task PublishNotificationAsync(NotificationMessage message, string routingKey);
    }
}
