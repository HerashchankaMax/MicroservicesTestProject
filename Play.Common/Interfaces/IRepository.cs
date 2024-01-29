using System.Linq.Expressions;

namespace Play.Common.Interfaces;

public interface IRepository<T> where T : IEntity
{
    Task<IReadOnlyCollection<T>> GetAll();
    Task<IReadOnlyCollection<T>> GetAll(Expression<Func<T, bool>> filter);
    Task<T> GetAsync(Guid id);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteASync(T entity);
}
