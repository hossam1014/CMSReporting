using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Repository;
using Application.Abstractions;
using Application.Contracts.MobileApp.MNotification;
using Application.Interfaces.MobileApp;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.MobileApp
{
    public class MNotificationRepo : IMNotificationRepo
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public MNotificationRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<MNotificationResponse>>> GetNotificationsAsync(string userId)
        {
            var notifications = await _context.NotificationUsers
                                        .Where(x => x.MobileUserId == userId)
                                        .Select(x => new MNotificationResponse(
                                            x.Notification.TitleAR,
                                            x.Notification.TitleEN,
                                            x.Notification.ContentAR,
                                            x.Notification.ContentEN,
                                            x.Notification.CreatedDate
                                        )).ToListAsync();

            return Result.Success(notifications);
        }
    }
}