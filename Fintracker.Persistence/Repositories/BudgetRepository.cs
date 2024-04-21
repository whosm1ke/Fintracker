using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Fintracker.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
    private readonly AppDbContext _db;

    public BudgetRepository(AppDbContext context) : base(context)
    {
        _db = context;
    }

    public async Task<Budget?> GetBudgetByIdAsync(Guid id)
    {
        return await _db.Budgets
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetBudgetsByCategoryId(Guid categoryId)
    {
        return await _db.Budgets
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.Categories
                .Any(x => x.Id == categoryId))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetByUserIdAsync(Guid userId, bool? isPublic)
    {
        var query = _db.Budgets
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .AsSplitQuery()
            .Where(x => x.UserId == userId);

        if (isPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == isPublic);
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetByWalletIdAsync(Guid walletId, bool? isPublic)
    {
        var query = _db.Budgets
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .AsSplitQuery()
            .Where(x => x.WalletId == walletId);

        if (isPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == isPublic);
        }

        return await query.ToListAsync();
    }


    public async Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(Guid userId, BudgetQueryParams queryParams)
    {
        return await _db.Budgets.GetByUserIdSortedAsync(userId, queryParams);
    }

    public async Task<IReadOnlyList<Budget>> GetByWalletIdSortedAsync(Guid walletId, BudgetQueryParams queryParams)
    {
        return await _db.Budgets.GetByWalletIdSortedAsync(walletId, queryParams);
    }
}