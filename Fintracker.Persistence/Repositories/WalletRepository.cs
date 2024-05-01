using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Fintracker.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
{
    private readonly AppDbContext _db;

    public WalletRepository(AppDbContext context) : base(context)
    {
        _db = context;
    }

    private IQueryable<Wallet> GetWalletQuery()
    {
        return _db.Wallets
            .Include(w => w.Users)
            .ThenInclude(x => x.UserDetails)
            .Include(w => w.Users)
            .ThenInclude(x => x.MemberBudgets)
            .ThenInclude(b => b.Categories)
            .Include(w => w.Users)
            .ThenInclude(x => x.MemberBudgets)
            .ThenInclude(b => b.Currency)
            .Include(w => w.Users)
            .ThenInclude(x => x.MemberBudgets)
            .ThenInclude(b => b.Transactions)
            .ThenInclude(t => t.Category)
            .Include(w => w.Users)
            .ThenInclude(x => x.MemberBudgets)
            .ThenInclude(b => b.Transactions)
            .ThenInclude(t => t.Currency)
            .Include(x => x.Owner)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Category)
            .AsSplitQuery();
    }

    public async Task<Wallet?> GetWalletById(Guid id)
    {
        return await GetWalletQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
    
    

    public async Task<IReadOnlyList<Wallet>> GetByUserIdAsync(Guid userId)
    {
        return await GetWalletQuery()
            .Where(x => x.OwnerId == userId || x.Users.Any(u => u.Id == userId))
            .ToListAsync();
    }
    
    

    public async Task<IReadOnlyList<Wallet>> GetByUserIdSortedAsync(Guid ownerId, QueryParams queryParams)
    {
        return await GetWalletQuery().GetByUserIdSortedAsync(ownerId, queryParams);
    }

    public async Task<Wallet?> GetWalletByBankAccount(string accountId)
    {
        return await _db.Wallets
            .Where(x => x.BankAccountId == accountId && x.IsBanking)
            .FirstOrDefaultAsync();
    }
}