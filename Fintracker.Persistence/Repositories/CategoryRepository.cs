using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Models;
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
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<Guid> GetDefaultBankIncomeCategoryId(Guid userId)
    {
        var incomeCategory = await _db.Categories
            .Where(x => x.IsSystemCategory && x.UserId == userId &&
                        x.Type == CategoryType.INCOME)
            .FirstOrDefaultAsync();


        if (incomeCategory is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "Unable to find system income category",
                PropertyName = "Id"
            }, nameof(Category));

        return incomeCategory.Id;
    }

    public async Task<Guid> GetDefaultBankExpenseCategoryId(Guid userId)
    {
        var expenseCategory = await _db.Categories
            .Where(x => x.IsSystemCategory && x.UserId == userId &&
                        x.Type == CategoryType.EXPENSE)
            .FirstOrDefaultAsync();

        if (expenseCategory is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "Unable to find system expense category",
                PropertyName = "Id"
            }, nameof(Category));

        return expenseCategory.Id;
    }

    public async Task<Category?> GetAsync(Guid userId, Guid id)
    {
        return await _db.Categories
            .Where(x => x.UserId == userId && x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Category>> GetByTypeAsync(Guid userId, CategoryType type)
    {
        return await _db.Categories
            .Where(c => c.UserId == userId)
            .Where(x => x.Type == type)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Category>> GetAllSortedAsync(Guid userId, QueryParams queryParams)
    {
        return await _db.Categories.GetAllSortedAsync(userId, queryParams);
    }

    public async Task<IReadOnlyCollection<Category>> GetAllByIds(ICollection<Guid> ids, Guid userId)
    {
        return await _db.Categories
            .Where(c => c.UserId == userId && ids.Contains(c.Id))
            .ToListAsync();
    }
}