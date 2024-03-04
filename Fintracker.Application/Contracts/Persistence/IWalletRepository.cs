using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetWalletWithOwnerAsync(Guid id);
    Task<Wallet?> GetWalletWithMembersAsync(Guid id);
    Task<Wallet?> GetWalletWithTransactionsAsync(Guid id);
    Task<Wallet?> GetWalletWithBudgetsAsync(Guid id);
    Task<IReadOnlyList<Wallet>> GetByOwnerIdAsync(Guid userId);

    Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(Guid userId, string sortBy);
}