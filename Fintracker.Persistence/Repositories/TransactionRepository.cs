using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Fintracker.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    private readonly AppDbContext _db;
    public TransactionRepository(AppDbContext context): base(context)
    {
        _db = context;
    }

    public async Task<Transaction?> GetTransactionAsync(Guid id)
    {
        return await _db.Transactions
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Transaction?> GetTransactionWithWalletAsync(Guid id)
    {
        return await _db.Transactions
            .Include(x => x.Wallet)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Transaction?> GetTransactionWithUserAsync(Guid id)
    {
        return await _db.Transactions
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId)
    {
        return await _db.Transactions
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdAsync(Guid walletId)
    {
        return await _db.Transactions
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Where(x => x.WalletId == walletId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _db.Transactions
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(Guid userId, string sortBy, bool isDescending)
    {
        return await _db.Transactions.GetByUserIdSortedAsync(userId, sortBy, isDescending);
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdSortedAsync(Guid walletId, string sortBy, bool isDescending)
    {
        return await _db.Transactions.GetByWalletIdSortedAsync(walletId, sortBy, isDescending);
    }

    public async Task<IReadOnlyList<Transaction>> GetByCategoryIdSortedAsync(Guid categoryId, string sortBy, bool isDescending)
    {
        return await _db.Transactions.GetByCategoryIdSortedAsync(categoryId, sortBy, isDescending);
    }

    public new async Task<IReadOnlyList<Transaction?>> GetAllAsync()
    {
        return await _db.Transactions
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .ToListAsync();
    }
}