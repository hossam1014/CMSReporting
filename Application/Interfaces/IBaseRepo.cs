using System.Linq.Expressions;
using Application.Helpers;

namespace Application.Interfaces
{
  public interface IBaseRepo<TEntity> where TEntity : class
  {
    TEntity Add(TEntity entity);
    void Remove(TEntity entity);
    void AddRange(List<TEntity> entity);
    void Update(TEntity entity);

    ValueTask<IEnumerable<TEntity>> GetAllAsync();
    ValueTask<IEnumerable<TEntity>> GetAllAsyncReadOnly();
    ValueTask<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression);
    ValueTask<bool> CheckExist(Expression<Func<TEntity, bool>> expression);
    ValueTask<int> CountAllByAsync(Expression<Func<TEntity, bool>> expression);
    ValueTask<int> SumAllBy(Expression<Func<TEntity, int>> sum,
        Expression<Func<TEntity, bool>> expression);

    ValueTask<TEntity> GetByIdAsync(int id);
    ValueTask<TEntity> GetByAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> include = null);

    ValueTask<TEntity> GetByAsyncReadOnly(Expression<Func<TEntity, bool>> expression);

    ValueTask<IEnumerable<T>> Map_GetAllAsync<T>();
    ValueTask<IEnumerable<T>> Map_GetAllAsyncNoTracking<T>();
    ValueTask<IEnumerable<T>> Map_GetAllAsyncNoTracking<T>(Expression<Func<TEntity, bool>> expression);
    ValueTask<IEnumerable<T>> Map_GetAllByAsync<T>(Expression<Func<TEntity, bool>> expression);

    // ValueTask<List<IEnumerable<TEntity>>> GetAllByAllAsync(Expression<Func<IEnumerable<TEntity>, bool>> expression);
    ValueTask<T> Map_GetByAsync<T>(Expression<Func<TEntity, bool>> expression);
    ValueTask<PagedList<T>> GetAllQueryableBy<T>(Expression<Func<TEntity, bool>> expression
        , Expression<Func<TEntity, object>> orderBy
        , PaginationParams paginationParams) where T : class;

    ValueTask<IQueryable<IGrouping<object, TEntity>>> GetAllQueryableGrouping(Expression<Func<TEntity, bool>> expression
    , Expression<Func<TEntity, object>> group, Expression<Func<TEntity, object>> include = null);
    ValueTask<TEntity> GetOrderByLast(Expression<Func<TEntity, object>> expression);
    ValueTask<T> Map_GetLastBy<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderBy);
    ValueTask<TEntity> GetOrderByLastBy(Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, bool>> expression);
    void RemoveAll(IEnumerable<TEntity> entity);
    void UpdateAll(IEnumerable<TEntity> entity);
    TEntity GetFirst();

    List<T> GetDistinct<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, T>> selector);

    ValueTask<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression);


  }
}