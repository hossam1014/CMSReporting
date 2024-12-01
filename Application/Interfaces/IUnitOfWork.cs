using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mapper { get; }

        IBaseRepo<TEntity> BaseRepo<TEntity>() where TEntity : BaseEntity;



        Task<bool> SaveAsync();
        bool HasChanges();
    }
}

