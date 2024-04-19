using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId);

    Task<User?> GetUserWithMemberWalletsByIdAsync(Guid id);

    Task<bool> HasMemberWallet(Guid walletId, string userEmail);

    Task<User> RegisterUserWithTemporaryPassword(string? email, Guid id, string tempPass);
    
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string email);
    Task UpdateAsync(User item);
    Task DeleteAsync(User item);

    // Task DeleteUserFromMemberWallet(Guid userId, Guid walletId);
    Task<User?> GetAsync(Guid id);
    Task<User?> GetAsNoTrackingAsync(string email);
    Task<User?> FindByEmailAsync(string email);
    Task<IReadOnlyList<User?>> GetAllAsync();
}