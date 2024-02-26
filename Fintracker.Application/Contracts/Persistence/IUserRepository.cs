using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId);

    Task<User?> GetUserWithWalletsByIdAsync(Guid id);
    Task<User?> GetUserWithBudgetsByIdAsync(Guid id);
}