using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Fintracker.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext context) : base(context)
    {
        _db = context;
    }
    
    private  IQueryable<Transaction> GetTransactionQuery()
    {
        return _db.Transactions
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
            .AsSplitQuery();
    }

    public async Task<Transaction?> GetTransactionAsync(Guid id)
    {
        return await GetTransactionQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Transaction?> GetTransactionWithWalletAsync(Guid id)
    {
        return await GetTransactionQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }


    public async Task<IReadOnlyList<Transaction>> GetAllAsync(Guid userId)
    {
        return await GetTransactionQuery()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId)
    {
        return await GetTransactionQuery()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdAsync(Guid walletId)
    {
        return await GetTransactionQuery()
            .Where(x => x.WalletId == walletId)
            .ToListAsync();
    }

 

    public async Task<IReadOnlyList<Transaction>> GetByCategoryIdAsync(Guid categoryId, Guid userId)
    {
        return await GetTransactionQuery()
            .Where(t => t.CategoryId == categoryId && t.UserId == userId)
            .ToListAsync();
    }
    

    public async Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(Guid userId,
        TransactionQueryParams queryParams)
    {
        
        return await GetTransactionQuery().GetByUserIdSortedAsync(userId, queryParams);
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdSortedAsync(Guid walletId,
        TransactionQueryParams queryParams)
    {
        return await GetTransactionQuery().GetByWalletIdSortedAsync(walletId, queryParams);
    }

    public async Task<IReadOnlyList<Transaction>> GetByWalletIdInRangeAsync(Guid walletId, DateTime budgetStart,
        DateTime budgetEnd)
    {
        return await _db.Transactions
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Budgets)
            .ThenInclude(x => x.Categories)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Where(x => x.WalletId == walletId && x.Date.Date >= budgetStart.Date && x.Date.Date <= budgetEnd.Date)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetByCategoryIdSortedAsync(Guid categoryId, Guid userId,
        TransactionQueryParams queryParams)
    {
        return await GetTransactionQuery().GetByCategoryIdSortedAsync(categoryId, userId, queryParams);
    }
}