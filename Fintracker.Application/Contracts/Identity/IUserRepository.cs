using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface IUserRepository
{
    Task<User> RegisterUserWithTemporaryPassword(string? email, Guid id, string tempPass);

    Task<bool> HasMemberWallet(Guid walletId, string userEmail);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string email);
    Task UpdateAsync(User item);
    Task DeleteAsync(User item);
    Task<User?> GetAsync(Guid id);
    Task<User?> GetAsNoTrackingAsync(string email);
    Task<User?> GetAsNoTrackingAsync(Guid userId);
    Task<User?> FindByEmailAsync(string email);
    Task<IReadOnlyList<User?>> GetAllAsync();
}