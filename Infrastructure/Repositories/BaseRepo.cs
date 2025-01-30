using System.Linq.Expressions;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
  public class BaseRepo<TEntity> : IBaseRepo<TEntity> where TEntity : BaseEntity
    //  where TEntity : class, BaseEntity
  {

    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly IMapper _mapper;

    public BaseRepo(DataContext context, IMapper mapper)
    {
      _context = context;
      _dbSet = _context.Set<TEntity>();
      _mapper = mapper;
    }

    public TEntity Add(TEntity entity)
    {
      _context.Set<TEntity>().Add(entity);
      return entity;
    }

    public Task AddRange(List<TEntity> entities)
    {
      _context.Set<TEntity>().AddRange(entities);
      return Task.CompletedTask;
    }

    public Task Update(TEntity entity)
    {
      _context.Update<TEntity>(entity);
      return Task.CompletedTask;
    }

    public Task Remove(TEntity entity)
    {
      _context.Set<TEntity>().Remove(entity);
      return Task.CompletedTask;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
      return await _context.Set<TEntity>().ToListAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>().Where(expression).ToListAsync();
    }
    public async Task<bool> CheckExist(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>().AnyAsync(expression);
    }
    public async Task<IEnumerable<T>> Map_GetAllAsync<T>()
    {
      return await _context.Set<TEntity>()
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .ToListAsync();
    }
    public async Task<IEnumerable<T>> Map_GetAllByAsync<T>(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>()
          .Where(expression)
          // .OrderByDescending(x => x.Id)
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .ToListAsync();
    }
    public async Task<T> Map_GetByAsync<T>(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>()
          .Where(expression)
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();
    }

  }
}