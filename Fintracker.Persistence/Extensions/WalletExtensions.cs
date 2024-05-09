using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class WalletExtensions
{
    
    public static async Task<IReadOnlyList<Wallet>> GetByUserIdSortedAsync(
        this IQueryable<Wallet> wallets,
        Guid userId,
        QueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Wallet), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var converted = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<Wallet, object>>(converted, parameter);

        // Apply the sorting to the query
        var baseQuery = wallets
            .Where(x => x.OwnerId == userId || x.Users.Any(u => u.Id == userId))
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize);

        var orderedQuery = queryParams.IsDescending
            ? baseQuery.OrderByDescending(lambda)
            : baseQuery.OrderBy(lambda);

        return await orderedQuery.ToListAsync();
    }
}