using Fintracker.Application.Models;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetWalletById(Guid id);
    Task<Wallet?> GetWalletWithMembersAsync(Guid id);
    Task<Wallet?> GetWalletWithTransactionsAsync(Guid id);
    Task<Wallet?> GetWalletWithBudgetsAsync(Guid id);
    Task<IReadOnlyList<Wallet>> GetByOwnerIdAsync(Guid ownerId);

    Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(Guid ownerId, QueryParams queryParams);
    Task<Wallet?> GetWalletByBankAccount(string accountId);
}