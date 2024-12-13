using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Dashboard;
using Application.Interfaces.MobileApp;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mapper { get; }
        IMReportRepo MReportRepo { get; }
        IMNotificationRepo MNotificationRepo { get; }
        IMEmergencyReportRepo MEmergencyReportRepo { get; }
        IAuthRepo AuthRepo { get; }
        IReportRepo ReportRepo { get; }

        IBaseRepo<TEntity> BaseRepo<TEntity>() where TEntity : BaseEntity;



        Task<bool> SaveAsync();
        bool HasChanges();
    }
}

