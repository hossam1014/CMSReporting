using System.Linq.Expressions;
using Application.Helpers;

namespace Application.Interfaces
{
  public interface IBaseRepo<T> where T : class
  {
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(object id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(object id);
  }

}