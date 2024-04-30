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

    public async Task<Wallet?> GetWalletById(Guid id)
    {
        return await _db.Wallets
            .Include(w => w.Users)
            .ThenInclude(x => x.UserDetails)
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
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Wallet?> GetWalletByIdWithMemberBudgets(Guid id)
    {
        return await _db.Wallets
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
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Wallet?> GetWalletByIdOnlyUsersAndBudgets(Guid id)
    {
        return await _db.Wallets
            .Include(x => x.Users)
            .Include(x => x.Budgets)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }


    public async Task<IReadOnlyList<Wallet>> GetByOwnerIdAsync(Guid ownerId)
    {
        return await _db.Wallets
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
            .AsSplitQuery()
            .Where(x => x.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Wallet>> GetByMemberIdAsync(Guid memberId)
    {
        return await _db.Wallets
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
            .AsSplitQuery()
            .Where(x => x.Users.Any(u => u.Id == memberId))
            .ToListAsync();
    }

    public async Task<Wallet?> GetWalletWithCurrency(Guid id)
    {
        return await _db.Wallets
            .Include(x => x.Currency)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(Guid ownerId, QueryParams queryParams)
    {
        return await _db.Wallets.GetByOwnerIdSortedAsync(ownerId, queryParams);
    }

    public async Task<IReadOnlyList<Wallet>> GetByMemberIdSortedAsync(Guid ownerId, QueryParams queryParams)
    {
        return await _db.Wallets.GetByMemberIdSortedAsync(ownerId, queryParams);
    }

    public async Task<Wallet?> GetWalletByBankAccount(string accountId)
    {
        return await _db.Wallets
            .Where(x => x.BankAccountId == accountId && x.IsBanking)
            .FirstOrDefaultAsync();
    }
}