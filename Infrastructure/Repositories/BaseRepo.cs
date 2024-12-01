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

    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public BaseRepo(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }

    public TEntity Add(TEntity entity)
    {
      _context.Set<TEntity>().Add(entity);
      return entity;
    }

    public void AddRange(List<TEntity> entities)
    {
      _context.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
      _context.Update<TEntity>(entity);

    }

    public void UpdateAll(IEnumerable<TEntity> entity)
    {
      _context.UpdateRange(entity);
    }

    public void Remove(TEntity entity)
    {
      _context.Set<TEntity>().Remove(entity);
    }

    public void RemoveAll(IEnumerable<TEntity> entity)
    {
      _context.Set<TEntity>().RemoveRange(entity);
    }

    public async ValueTask<IEnumerable<TEntity>> GetAllAsync()
    {
      return await _context.Set<TEntity>().ToListAsync();
    }

    public async ValueTask<IEnumerable<TEntity>> GetAllAsyncReadOnly()
    {
      return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async ValueTask<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>().Where(expression).ToListAsync();
    }

    public async ValueTask<bool> CheckExist(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>().AnyAsync(expression);
    }

    public async ValueTask<int> CountAllByAsync(Expression<Func<TEntity, bool>> expression)
    {
      var result = _context.Set<TEntity>().Where(expression);

      return await result.CountAsync();

    }

    public async ValueTask<int> SumAllBy(Expression<Func<TEntity, int>> sum,
        Expression<Func<TEntity, bool>> expression)
    {
      var entity = _context.Set<TEntity>().Where(expression);

      return await entity.SumAsync(sum);
    }


    public async ValueTask<TEntity> GetByAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> include = null)
    {
      IQueryable<TEntity> query = _context.Set<TEntity>();

      if (include != null)
      {
        query = query.Include(include);
      }

      return await query.FirstOrDefaultAsync(expression);

    }

    public async ValueTask<TEntity> GetByAsyncReadOnly(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(expression);

    }


    public async ValueTask<TEntity> GetOrderByLastBy(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>().OrderBy(orderBy).LastOrDefaultAsync(expression);
    }

    public async ValueTask<T> Map_GetLastBy<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderBy)
    {
      return await _context.Set<TEntity>()
        .OrderBy(orderBy)
        .Where(expression)
        .ProjectTo<T>(_mapper.ConfigurationProvider)
        .LastOrDefaultAsync();
    }

    public async ValueTask<TEntity> GetOrderByLast(Expression<Func<TEntity, object>> expression)
    {
      return await _context.Set<TEntity>().OrderBy(expression).LastOrDefaultAsync();
    }



    public async ValueTask<TEntity> GetByIdAsync(int id)
    {
      return await _context.Set<TEntity>().FindAsync(id);
    }

    public async ValueTask<IEnumerable<T>> Map_GetAllAsync<T>()
    {
      return await _context.Set<TEntity>()
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .ToListAsync();
    }
    public async ValueTask<IEnumerable<T>> Map_GetAllAsyncNoTracking<T>()
    {
      return await _context.Set<TEntity>()
          .Where(x => x.IsDeleted == false)
          .OrderByDescending(x => x.Id)
          .AsNoTracking()
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .ToListAsync();
    }

    public async ValueTask<IEnumerable<T>> Map_GetAllAsyncNoTracking<T>(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>()
          .Where(x => x.IsDeleted == false)
          .Where(expression)
          .OrderByDescending(x => x.Id)
          .AsNoTracking()
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .ToListAsync();
    }

    public async ValueTask<IEnumerable<T>> Map_GetAllByAsync<T>(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>()
          .Where(expression)
          // .OrderByDescending(x => x.Id)
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .ToListAsync();
    }


    public async ValueTask<T> Map_GetByAsync<T>(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>()
          .Where(expression)
          .ProjectTo<T>(_mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();
    }


    public async ValueTask<PagedList<T>> GetAllQueryableBy<T>(Expression<Func<TEntity, bool>> expression
        , Expression<Func<TEntity, object>> orderBy
        , PaginationParams paginationParams) where T : class
    {
      var query = _context.Set<TEntity>()
          // .AsQueryable()
          .Where(expression)
          .OrderByDescending(orderBy)
          .AsNoTracking();

      // query = query.AsSplitQuery();

      var queryToCreate = query.ProjectTo<T>(_mapper.ConfigurationProvider);

      var result = await PagedList<T>.CreateAsync(queryToCreate, paginationParams);

      return result;

    }


    public async ValueTask<IQueryable<IGrouping<object, TEntity>>> GetAllQueryableGrouping(Expression<Func<TEntity, bool>> expression
    , Expression<Func<TEntity, object>> group, Expression<Func<TEntity, object>> include = null)
    {
      var query = _context.Set<TEntity>()
                    .Where(expression)
                    .AsQueryable();

      if (include != null)
      {
        query = query.Include(include);
      }

      var groupedResult = query.GroupBy(group.Compile()).AsQueryable();
      return await ValueTask.FromResult(groupedResult);
    }

    public TEntity GetFirst()
    {
      return _context.Set<TEntity>().FirstOrDefault();
    }

    public List<T> GetDistinct<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, T>> selector)
    {
      var distinctYears = _context.Set<TEntity>()
          .Where(expression)
          .Select(selector)
          .Distinct()
          .OrderByDescending(x => x)
          .ToList();

      return distinctYears;
    }

    public async ValueTask<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
    {
      return await _context.Set<TEntity>().AnyAsync(expression);
    }

        public ValueTask<IQueryable<TEntity>> GetAllQueryableByX(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> orderBy)
        {
            throw new NotImplementedException();
        }

        public ValueTask<double> GetAllQueryableSumCol(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, double>> sum)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IQueryable<T>> map_GetLastByNum<T>(int num, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderBy) where T : class
        {
            throw new NotImplementedException();
        }

        public ValueTask<IQueryable<object>> Map_GetAllNamesByAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> selector)
        {
            throw new NotImplementedException();
        }
    }
}