using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity<Guid>
{
    private readonly AppDbContext _db;
    public GenericRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<T?> GetAsync(Guid id)
    {
        return await _db.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetAsyncNoTracking(Guid id)
    {
        return await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<T?>> GetAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T?>> GetAllAsyncNoTracking()
    {
        return await _db.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T> AddAsync(T item)
    {
        await _db.AddAsync(item);
        return item;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await GetAsyncNoTracking(id) != null;
    }

    public async Task UpdateAsync(T item)
    {
        _db.Entry(item).State = EntityState.Modified;
    }

    public async Task DeleteAsync(T item)
    {
        _db.Set<T>().Remove(item);
    }
}