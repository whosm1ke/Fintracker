using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Exceptions;
using Fintracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Identity.Services;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> context)
    {
        _userManager = context;
    }

    public async Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId)
    {
        return await _userManager.Users
            .Where(x => x.MemberWallets.Any(x => x.Id == walletId))
            .ToListAsync();
    }

    public async Task<User?> GetUserWithOwnedWalletsByIdAsync(Guid id)
    {
        return await _userManager.Users
            .Include(x => x.OwnedWallets)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithMemberWalletsByIdAsync(Guid id)
    {
        return await _userManager.Users
            .Include(x => x.MemberWallets)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithBudgetsByIdAsync(Guid id)
    {
        return await _userManager.Users
            .Include(x => x.Budgets)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> HasMemberWallet(Guid walletId, string userEmail)
    {
        return await _userManager.Users
            .AsNoTracking()
            .Where(x => x.Email == userEmail && x.MemberWallets.Any(x => x.Id == walletId))
            .FirstOrDefaultAsync() != null;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString()) != null;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task UpdateAsync(User item)
    {
        await _userManager.UpdateAsync(item);
    }

    public async Task DeleteAsync(User item)
    {
        await _userManager.DeleteAsync(item);
    }


    public async Task<User?> GetAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<User?> GetAsNoTrackingAsync(string email)
    {
        return await _userManager.Users
            .AsNoTracking()
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<User> RegisterUserWithTemporaryPassword(string? email, Guid id, string tempPass)
    {
        if (email is null || id == Guid.Empty)
            throw new BadRequestException(
                $"Can not register new user. Invalid param '{nameof(email)}' or '{nameof(id)}'");

        var user = new User()
        {
            UserName = email,
            Email = email,
            Id = id
        };
        var userResult = await _userManager.CreateAsync(user, tempPass);

        if (userResult.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
                throw new BadRequestException(roleResult.Errors.Select(x => x.Description).ToList());

            return user;
        }

        throw new BadRequestException(userResult.Errors.Select(x => x.Description).ToList());
    }

    public async Task<IReadOnlyList<User?>> GetAllAsync()
    {
        return await _userManager.Users.ToListAsync();
    }
}