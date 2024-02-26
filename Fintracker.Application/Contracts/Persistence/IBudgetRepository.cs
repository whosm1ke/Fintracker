using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IBudgetRepository : IGenericRepository<Budget>
{
    Task<Budget?> GetBudgetAsync(Guid id);
    Task<Budget?> GetBudgetWithWalletAsync(Guid id);
    Task<IReadOnlyList<Budget>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<Budget>> GetByWalletIdAsync(Guid walletId);

    Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(Guid userId, string sortBy);
}