using Fintracker.Application.Contracts.Persistence;
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
            .Include(x => x.Owner)
            .Include(x => x.Currency)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }


    public async Task<Wallet?> GetWalletWithMembersAsync(Guid id)
    {
        return await _db.Wallets
            .Include(x => x.Owner)
            .Include(x => x.Currency)
            .Include(x => x.Users)
            .ThenInclude(x => x.UserDetails)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Wallet?> GetWalletWithTransactionsAsync(Guid id)
    {
        return await _db.Wallets
            .Include(x => x.Owner)
            .Include(x => x.Currency)
            .Include(x => x.Transactions)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Wallet?> GetWalletWithBudgetsAsync(Guid id)
    {
        return await _db.Wallets
            // .Include(x => x.Owner)
            .Include(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Wallet>> GetByOwnerIdAsync(Guid ownerId)
    {
        return await _db.Wallets
            .Include(x => x.Owner)
            .Include(x => x.Currency)
            .Where(x => x.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(Guid ownerId, string sortBy, bool isDescending)
    {
        return await _db.Wallets.GetByOwnerIdSortedAsync(ownerId, sortBy, isDescending);
    }

    public async Task<Wallet?> GetWalletByBankAccount(string accountId)
    {
        return await _db.Wallets
            .Where(x => x.BankAccountId == accountId && x.IsBanking)
            .FirstOrDefaultAsync();
    }
}