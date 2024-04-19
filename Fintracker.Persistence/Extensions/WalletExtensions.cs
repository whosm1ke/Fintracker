using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class WalletExtensions
{
    public static async Task<IReadOnlyList<Wallet>> GetByOwnerIdSortedAsync(
        this DbSet<Wallet> wallets,
        Guid ownerId,
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
        var query = queryParams.IsDescending
            ? wallets
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Include(x => x.Owner)
                .ThenInclude(x => x.UserDetails)
                .Include(x => x.Currency)
                .Include(x => x.Budgets)
                .ThenInclude(x => x.Currency)
                .Include(x => x.Budgets)
                .ThenInclude(x => x.Categories)
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Currency)
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Category)
                .AsSplitQuery()
                .Where(x => x.OwnerId == ownerId)
                .OrderByDescending(lambda)
            : wallets
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Include(x => x.Owner)
                .ThenInclude(x => x.UserDetails)
                .Include(x => x.Currency)
                .Include(x => x.Budgets)
                .ThenInclude(x => x.Currency)
                .Include(x => x.Budgets)
                .ThenInclude(x => x.Categories)
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Currency)
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Category)
                .AsSplitQuery()
                .Where(x => x.OwnerId == ownerId)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }
}