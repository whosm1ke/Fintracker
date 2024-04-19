using Fintracker.Application.Models;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IBudgetRepository : IGenericRepository<Budget>
{
    Task<Budget?> GetBudgetByIdAsync(Guid id);

    Task<IReadOnlyList<Budget>> GetBudgetsByCategoryId(Guid categoryId);
    Task<IReadOnlyList<Budget>> GetByUserIdAsync(Guid userId, bool? isPublic);
    Task<IReadOnlyList<Budget>> GetByWalletIdAsync(Guid walletId, bool? isPublic);

    Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(Guid userId, QueryParams queryParams, bool? isPublic);
    Task<IReadOnlyList<Budget>> GetByWalletIdSortedAsync(Guid walletId, QueryParams queryParams, bool? isPublic);
}