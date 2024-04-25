using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Models;
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
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Transaction?> GetTransactionWithWalletAsync(Guid id)
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
    

    public async Task<IReadOnlyList<Transaction>> GetAllAsync(Guid userId)
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId)
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdAsync(Guid walletId)
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.WalletId == walletId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(Guid userId, TransactionQueryParams queryParams)
    {
        return await _db.Transactions.GetByUserIdSortedAsync(userId, queryParams);
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdSortedAsync(Guid walletId, TransactionQueryParams queryParams)
    {
        
        return await _db.Transactions.GetByWalletIdSortedAsync(walletId, queryParams);
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdInRangeAsync(Guid walletId, DateTime budgetStart, DateTime budgetEnd)
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Where(x => x.WalletId == walletId && x.Date >= budgetStart && x.Date <= budgetEnd)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByCategoryIdSortedAsync(Guid categoryId, TransactionQueryParams queryParams)
    {
        return await _db.Transactions.GetByCategoryIdSortedAsync(categoryId, queryParams);
    }
    


    public new async Task<IReadOnlyList<Transaction?>> GetAllAsync()
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .ToListAsync();
    }
}