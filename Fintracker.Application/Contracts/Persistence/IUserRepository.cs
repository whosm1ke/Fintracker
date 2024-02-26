using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}