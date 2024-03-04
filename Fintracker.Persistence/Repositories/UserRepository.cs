using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext context): base(context)
    {
        _db = context;
    }

    public async Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId)
    {
        return await _db.Users
            .Where(x => x.Wallets.Any(x => x.Id == walletId))
            .ToListAsync();
    }

    public async Task<User?> GetUserWithWalletsByIdAsync(Guid id)
    {
        return await _db.Users
            .Include(x => x.Wallets)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithBudgetsByIdAsync(Guid id)
    {
        return await _db.Users
            .Include(x => x.Budgets)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}