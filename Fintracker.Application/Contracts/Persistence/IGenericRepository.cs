using Fintracker.Domain.Common;

namespace Fintracker.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class, IEntity<Guid>
{
    Task<T?> GetAsync(Guid id);
    Task<T?> GetAsyncNoTracking(Guid id);
    Task<IReadOnlyList<T?>> GetAllAsync();
    Task<IReadOnlyList<T?>> GetAllAsyncNoTracking();
    Task<T> AddAsync(T item);
    Task<bool> ExistsAsync(Guid id);
    void Update(T item);
    void Delete(T item);
}