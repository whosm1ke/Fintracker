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
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetBudgetsByCategoryId(Guid categoryId)
    {
        return await _db.Budgets
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Category)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Where(x => x.Categories
                .Any(x => x.Id == categoryId))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetByBudgetOwnerIdAsync(Guid userId, bool? isPublic)
    {
        var query = _db.Budgets
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Category)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.Owner)
            .ThenInclude(x => x.UserDetails)
            .AsSplitQuery()
            .Where(x => x.OwnerId == userId);

        if (isPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == isPublic);
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<Budget>> GetByBudgetUserIdAsync(Guid userId, bool? isPublic)
    {
        var user = await _db.Users
            .Include(user => user.OwnedBudgets)
            .ThenInclude(b => b.Categories)
            .Include(user => user.OwnedBudgets)
            .ThenInclude(b => b.Transactions)
            .ThenInclude(t => t.Currency)
            .Include(user => user.OwnedBudgets)
            .ThenInclude(b => b.Transactions)
            .ThenInclude(t => t.Category)
            .Include(user => user.OwnedBudgets)
            .ThenInclude(b => b.Owner)
            .ThenInclude(u => u.UserDetails)
            .Include(user => user.OwnedBudgets)
            .ThenInclude(b => b.Members)
            .ThenInclude(u => u.UserDetails)
            .Include(user => user.OwnedBudgets)
            .ThenInclude(b => b.Wallet)
            .Include(user => user.MemberBudgets)
            .ThenInclude(b => b.Categories)
            .Include(user => user.MemberBudgets)
            .ThenInclude(b => b.Transactions)
            .ThenInclude(t => t.Currency)
            .Include(user => user.MemberBudgets)
            .ThenInclude(b => b.Transactions)
            .ThenInclude(t => t.Category)
            .Include(user => user.MemberBudgets)
            .ThenInclude(b => b.Owner)
            .ThenInclude(u => u.UserDetails)
            .Include(user => user.MemberBudgets)
            .ThenInclude(b => b.Members)
            .ThenInclude(u => u.UserDetails)
            .Include(user => user.MemberBudgets)
            .ThenInclude(b => b.Wallet)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new List<Budget>();
        }

        var ownedBudgets = user.OwnedBudgets.AsQueryable();
        var memberBudgets = user.MemberBudgets.AsQueryable();

        if (isPublic.HasValue)
        {
            ownedBudgets = ownedBudgets.Where(b => b.IsPublic == isPublic.Value);
            memberBudgets = memberBudgets.Where(b => b.IsPublic == isPublic.Value);
        }

        var budgets = ownedBudgets.Union(memberBudgets).DistinctBy(b => b.Id);

        return budgets.ToList();
    }


    public async Task<IReadOnlyList<Budget>> GetByWalletIdAsync(Guid walletId, Guid userId, bool? isPublic)
    {
        var query = _db.Budgets
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Category)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.Owner)
            .ThenInclude(x => x.UserDetails)
            .AsSplitQuery()
            .Where(x => x.WalletId == walletId && x.OwnerId == userId);

        if (isPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == isPublic);
        }

        return await query.ToListAsync();
    }


    public async Task<IReadOnlyList<Budget>> GetByOwnerIdSortedAsync(Guid userId, BudgetQueryParams queryParams)
    {
        return await _db.Budgets.GetByOwnerIdSortedAsync(userId, queryParams);
    }

    public async Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(Guid userId, BudgetQueryParams queryParams)
    {
        return await _db.Users.GetByUserIdSortedAsync(userId, queryParams);
    }

    public async Task<IReadOnlyList<Budget>> GetByWalletIdSortedAsync(Guid walletId, Guid userId,
        BudgetQueryParams queryParams)
    {
        return await _db.Budgets.GetByWalletIdSortedAsync(walletId, userId, queryParams);
    }
}