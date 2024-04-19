using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Exceptions;
using Fintracker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Identity.Services;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly Guid _currentUserId;

    public UserRepository(UserManager<User> context, IHttpContextAccessor accessor)
    {
        _userManager = context;
        var uid = accessor.HttpContext!.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypeConstants.Uid)?.Value;
        if(!Guid.TryParse(uid, out _currentUserId))
            _currentUserId = Guid.Empty;
    }

    public async Task<IReadOnlyList<User>> GetAllAccessedToWalletAsync(Guid walletId)
    {
        return await _userManager.Users
            .Where(x => x.Id == _currentUserId && x.MemberWallets.Any(w => w.Id == walletId))
            .ToListAsync();
    }
    
    public async Task<User?> GetUserWithMemberWalletsByIdAsync(Guid id)
    {
        return await _userManager.Users
            .Include(x => x.MemberWallets)
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

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User> RegisterUserWithTemporaryPassword(string? email, Guid id, string tempPass)
    {
        if (email is null || id == Guid.Empty)
            throw new BadRequestException(new List<ExceptionDetails>
            {
                new()
                {
                    PropertyName = nameof(email),
                    ErrorMessage = "Invalid email. Check spelling"
                },
                new()
                {
                    PropertyName = nameof(id),
                    ErrorMessage = "Attempt to assign incorrect id"
                }
            });

        var user = new User
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
                throw new BadRequestException(roleResult.Errors.Select(x => new ExceptionDetails
                {
                    ErrorMessage = x.Description,
                    PropertyName = null
                }).ToList());

            return user;
        }

        throw new BadRequestException(userResult.Errors.Select(x => new ExceptionDetails
        {
            ErrorMessage = x.Description,
            PropertyName = null
        }).ToList());
    }

    public async Task<IReadOnlyList<User?>> GetAllAsync()
    {
        return await _userManager.Users.ToListAsync();
    }
}