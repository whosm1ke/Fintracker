using Fintracker.Application.Models;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetWalletById(Guid id);
    Task<Wallet?> GetWalletByIdWithMemberBudgets(Guid id);
    Task<Wallet?> GetWalletByIdOnlyUsersAndBudgets(Guid id);
    Task<IReadOnlyList<Wallet>> GetByOwnerIdAsync(Guid ownerId);
    Task<IReadOnlyList<Wallet>> GetByMemberIdAsync(Guid memberId);

    Task<Wallet?> GetWalletWithCurrency(Guid id);

    Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(Guid ownerId, QueryParams queryParams);
    Task<IReadOnlyList<Wallet>> GetByMemberIdSortedAsync(Guid memberId, QueryParams queryParams);
    Task<Wallet?> GetWalletByBankAccount(string accountId);
}