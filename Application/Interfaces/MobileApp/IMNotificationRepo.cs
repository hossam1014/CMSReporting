using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.MobileApp.MNotification;

namespace Application.Interfaces.MobileApp
{
    public interface IMNotificationRepo
    {
        public Task<Result<List<MNotificationResponse>>> GetNotificationsAsync(string userId);
    }
}