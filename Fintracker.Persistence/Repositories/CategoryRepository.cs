﻿using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Exceptions;
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

    public async Task<Guid> GetDefaultBankIncomeCategoryId()
    {
        var incomeCategory = await _db.Categories
            .Where(x => x.UserId == null &&
                        x.Name == "Income" && x.Type == CategoryType.INCOME)
            .FirstOrDefaultAsync();

        if (incomeCategory is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "System category for bank income was not found",
                PropertyName = ""
            }, nameof(Category));

        return incomeCategory.Id;
    }

    public async Task<Guid> GetDefaultBankExpenseCategoryId()
    {
        var expenseCategory = await _db.Categories
            .Where(x => x.UserId == null &&
                        x.Name == "Expense" && x.Type == CategoryType.EXPENSE)
            .FirstOrDefaultAsync();

        if (expenseCategory is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "System category for bank expense was not found",
                PropertyName = ""
            }, nameof(Category));

        return expenseCategory.Id;
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
        return await _db.Categories.GetAllSortedAsync(userId, sortBy, isDescending);
    }

    public async Task<IReadOnlyCollection<Category>> GetAllWithIds(ICollection<Guid> ids)
    {
        return await _db.Categories
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();
    }
}