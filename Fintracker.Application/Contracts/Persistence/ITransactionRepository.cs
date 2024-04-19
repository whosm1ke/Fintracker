using Fintracker.Application.Models;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<Transaction?> GetTransactionAsync(Guid id);
    Task<Transaction?> GetTransactionWithWalletAsync(Guid id);

    Task<IReadOnlyList<Transaction>> GetAllAsync(Guid userId);
    Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<Transaction>> GetByWalletIdAsync(Guid walletId);
    Task<IReadOnlyList<Transaction>> GetByCategoryIdAsync(Guid categoryId);
    Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(Guid userId, TransactionQueryParams queryParams);
    Task<IReadOnlyList<Transaction>> GetByWalletIdSortedAsync(Guid walletId, TransactionQueryParams queryParams);
    Task<IReadOnlyList<Transaction>> GetByCategoryIdSortedAsync(Guid categoryId, TransactionQueryParams queryParams);
}