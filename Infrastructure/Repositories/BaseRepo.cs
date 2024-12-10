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
  public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
    //  where TEntity : class, BaseEntity
  {

    protected readonly DataContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepo(DataContext context)
    {
      _context = context;
      _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
      return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(object id)
    {
      return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
      return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
      await _dbSet.AddAsync(entity);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
      _dbSet.Update(entity);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(object id)
    {
      var entity = await GetByIdAsync(id);
      if (entity != null)
      {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
      }
    }

  }
}