using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<Transaction?> GetTransactionAsync(Guid id);
    Task<Transaction?> GetTransactionWithWalletAsync(Guid id);
    Task<IReadOnlyList<Transaction>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<Transaction>> GetByWalletIdAsync(Guid walletId);
    Task<IReadOnlyList<Transaction>> GetByCategoryIdAsync(Guid categoryId);
    Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(Guid userId, string sortBy);
}