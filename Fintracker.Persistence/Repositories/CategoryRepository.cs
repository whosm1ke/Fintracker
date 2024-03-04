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

    public async Task<IReadOnlyList<Category>> GetByTypeAsync(CategoryType type)
    {
        return await _db.Categories.Where(x => x.Type == type).ToListAsync();
    }

    public async Task<IReadOnlyList<Category>> GetAllSortedAsync(string sortBy, bool isDescending)
    {
        return await _db.Categories.GetAllSortedAsync(sortBy, isDescending);
    }
}