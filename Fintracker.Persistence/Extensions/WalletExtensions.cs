using System.Linq.Expressions;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class WalletExtensions
{
    public static async Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(
        this DbSet<Wallet> wallets,
        Guid ownerId,
        string sortBy,
        bool isDescending)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Wallet), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, sortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Wallet, object>>(property, parameter);

        // Apply the sorting to the query
        var query = isDescending
            ? wallets.Where(x => x.OwnerId == ownerId).OrderByDescending(lambda)
            : wallets.Where(x => x.OwnerId == ownerId).OrderBy(lambda);

        return await query.ToListAsync();
    }
    
    
   
}