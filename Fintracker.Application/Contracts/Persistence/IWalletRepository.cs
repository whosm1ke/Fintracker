using Fintracker.Application.Models;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetWalletById(Guid id);
    Task<Wallet?> GetWalletByIdOnlyUsers(Guid id);
    Task<IReadOnlyList<Wallet>> GetByOwnerIdAsync(Guid ownerId);

    Task<Wallet?> GetWalletWithCurrency(Guid id);

    Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(Guid ownerId, QueryParams queryParams);
    Task<Wallet?> GetWalletByBankAccount(string accountId);
}