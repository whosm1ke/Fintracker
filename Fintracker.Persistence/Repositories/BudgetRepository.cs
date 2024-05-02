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

    private IQueryable<Budget> GetBudgetQuery()
    {
        return _db.Budgets
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Category)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Transactions)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Budgets)
            .Include(x => x.Owner)
            .ThenInclude(x => x.UserDetails)
            .Include(b => b.Members)
            .ThenInclude(m => m.UserDetails)
            .AsSplitQuery();
    }

    public async Task<Budget?> GetBudgetByIdAsync(Guid id)
    {
        return await GetBudgetQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetBudgetsByCategoryId(Guid categoryId, Guid userId)
    {
        return await GetBudgetQuery()
            .Where(x => x.Categories
                .Any(x => x.Id == categoryId) &&  x.OwnerId == userId || x.Members.Any(m => m.Id == userId) )
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetBudgetsByUserIdAsync(Guid userId, bool? isPublic)
    {
        var query = GetBudgetQuery().Where(b => b.OwnerId == userId || b.Members.Any(m => m.Id == userId));
        if (isPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == isPublic);
        }

        return await query.ToListAsync();
    }
    


    public async Task<IReadOnlyList<Budget>> GetByWalletIdAsync(Guid walletId, Guid userId, bool? isPublic)
    {
        var query = GetBudgetQuery()
            .Where(x => x.WalletId == walletId &&  x.OwnerId == userId || x.Members.Any(m => m.Id == userId));

        if (isPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == isPublic);
        }

        return await query.ToListAsync();
    }
    

    public async Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(Guid userId, BudgetQueryParams queryParams)
    {
        return await GetBudgetQuery().GetByUserIdSortedAsync(userId, queryParams);
    }

    public async Task<IReadOnlyList<Budget>> GetByWalletIdSortedAsync(Guid walletId, Guid userId,
        BudgetQueryParams queryParams)
    {
        return await GetBudgetQuery().GetByWalletIdSortedAsync(walletId, userId, queryParams);
    }
}