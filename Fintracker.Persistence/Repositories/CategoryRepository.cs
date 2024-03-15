using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;
using Fintracker.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _db = context;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(Guid userId)
    {
        return await _db.Categories
            .Where(c => c.UserId == null || c.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Category>> GetByTypeAsync(Guid userId, CategoryType type)
    {
        return await _db.Categories
            .Where(c => c.UserId == null || c.UserId == userId)
            .Where(x => x.Type == type)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Category>> GetAllSortedAsync(Guid userId, string sortBy, bool isDescending)
    {
        return await _db.Categories.GetAllSortedAsync(userId,sortBy, isDescending);
    }

    public async Task<IReadOnlyCollection<Category>> GetAllWithIds(ICollection<Guid> ids)
    {
        return await _db.Categories
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();
    }
}