using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId);

    Task<User?> GetUserWithOwnedWalletsByIdAsync(Guid id);
    Task<User?> GetUserWithMemberWalletsByIdAsync(Guid id);
    Task<User?> GetUserWithBudgetsByIdAsync(Guid id);

    Task<bool> HasMemberWallet(Guid walletId);
    
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string email);
    Task UpdateAsync(User item);
    Task DeleteAsync(User item);
    Task<User?> GetAsync(Guid id);
    Task<User?> GetAsNoTrackingAsync(string email);
    Task<IReadOnlyList<User?>> GetAllAsync();
}