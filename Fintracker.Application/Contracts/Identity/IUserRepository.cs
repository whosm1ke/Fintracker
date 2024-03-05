using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId);

    Task<User?> GetUserWithWalletsByIdAsync(Guid id);
    Task<User?> GetUserWithBudgetsByIdAsync(Guid id);
    
    Task<bool> ExistsAsync(Guid id);
    Task UpdateAsync(User item);
    Task DeleteAsync(User item);
    
    Task<User?> GetAsync(Guid id);
    Task<IReadOnlyList<User?>> GetAllAsync();
}