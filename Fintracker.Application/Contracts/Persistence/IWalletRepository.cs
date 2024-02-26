using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetWalletAsync(Guid id);
    Task<Wallet?> GetWalletWithUsersAndOwnerAsync(Guid id);
    Task<Wallet?> GetWalletWithTransactionsAsync(Guid id);
    Task<Wallet?> GetWalletWithBudgetsAsync(Guid id);
    Task<IReadOnlyList<Wallet>> GetByUserIdAsync(Guid userId);

    Task<IReadOnlyList<Wallet>> GetByUserIdSortedAsync(Guid userId, string sortBy);
}