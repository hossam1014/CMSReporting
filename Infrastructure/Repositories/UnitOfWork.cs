// using System.Collections;
// using API.Repository;
// using Application.Interfaces;
// using Application.Interfaces.Dashboard;
// using Application.Interfaces.MobileApp;
// using Application.Repositories.Dashboard;
// using Application.Repositories.MobileApp;
// using AutoMapper;
// using Domain.Entities;
// using Infrastructure.Data;
// using Infrastructure.Repositories.Dashboard;
// using Infrastructure.Repositories.MobileApp;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Hosting;

// namespace API.Data.Repository
// {
//     public class UnitOfWork : IUnitOfWork
//     {
//         private readonly IHostEnvironment _hostEnvironment;

//         private Hashtable _repositories;
//         private readonly DataContext _context;
//         private readonly IMapper _mapper;
//         private IHttpContextAccessor _httpContextAccessor;


//         public UnitOfWork(DataContext context, IMapper mapper,
//             IHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
//         {
//             _hostEnvironment = hostEnvironment;
//             _mapper = mapper;
//             _context = context;
//             _httpContextAccessor = httpContextAccessor;
//         }

//         public IMapper Mapper => _mapper;
//         public IMReportRepo MReportRepo => new MReportRepo(_context, _mapper);
//         public IMNotificationRepo MNotificationRepo => new MNotificationRepo(_context, _mapper);
//         public IMEmergencyReportRepo MEmergencyReportRepo => new MEmergencyReportRepo(_context, _mapper);
//         public IReportRepo ReportRepo => new ReportRepo(_context, _mapper, _httpContextAccessor);


//         public async Task<bool> SaveAsync()
//         {
//             return await _context.SaveChangesAsync() > 0;
//         }

//         public void Dispose()
//         {
//             _context.Dispose();
//         }

//         public bool HasChanges()
//         {
//             return _context.ChangeTracker.HasChanges();
//         }

//         public IBaseRepo<TEntity> BaseRepo<TEntity>() where TEntity : BaseEntity
//         {
//             if (_repositories == null) _repositories = new Hashtable();

//             var type = typeof(TEntity).Name;

//             if (!_repositories.ContainsKey(type))
//             {
//                 var repositoryType = typeof(BaseRepo<>);
//                 var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, _mapper);

//                 _repositories.Add(type, repositoryInstance);
//             }

//             return (BaseRepo<TEntity>)_repositories[type];
//         }



//     }
// }