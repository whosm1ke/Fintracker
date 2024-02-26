using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId);
}