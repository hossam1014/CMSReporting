using System.Linq.Expressions;
using Application.Helpers;

namespace Application.Interfaces
{
  public interface IBaseRepo<TEntity> where TEntity : class
  {
    
    TEntity Add(TEntity entity);
    Task AddRange(List<TEntity> entity);
    Task Remove(TEntity entity);
    Task Update(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> expression);

    Task<bool> CheckExist(Expression<Func<TEntity, bool>> expression);

    Task<IEnumerable<T>> Map_GetAllAsync<T>();

    Task<IEnumerable<T>> Map_GetAllByAsync<T>(Expression<Func<TEntity, bool>> expression);

    Task<T> Map_GetByAsync<T>(Expression<Func<TEntity, bool>> expression);

  }

}