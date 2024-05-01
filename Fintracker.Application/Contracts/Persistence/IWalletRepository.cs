using Fintracker.Application.Models;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetWalletById(Guid id);
    
    Task<IReadOnlyList<Wallet>> GetByUserIdAsync(Guid userId);

    Task<IReadOnlyList<Wallet>> GetByUserIdSortedAsync(Guid userId, QueryParams queryParams);
    Task<Wallet?> GetWalletByBankAccount(string accountId);
}