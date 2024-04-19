using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class TransactionExtensions
{
    public static async Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(
        this DbSet<Transaction> transactions,
        Guid userId,
        TransactionQueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Transaction), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var converted = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<Transaction, object>>(converted, parameter);

        // Apply the sorting to the query
        // Common query parts
        var baseQuery = transactions
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.UserId == userId && (x.Date >= queryParams.StartDate && x.Date <= queryParams.EndDate));

// Apply ordering
        var orderedQuery = queryParams.IsDescending
            ? baseQuery.OrderByDescending(lambda)
            : baseQuery.OrderBy(lambda);

        return await orderedQuery.ToListAsync();

    }

    public static async Task<IReadOnlyList<Transaction>> GetByWalletIdSortedAsync(
        this DbSet<Transaction> transactions,
        Guid walletId,
        TransactionQueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Transaction), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var converted = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<Transaction, object>>(converted, parameter);

        // Apply the sorting to the query
        var baseQuery = transactions
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.WalletId == walletId && (x.Date >= queryParams.StartDate && x.Date <= queryParams.EndDate));

// Apply ordering
        var orderedQuery = queryParams.IsDescending
            ? baseQuery.OrderByDescending(lambda)
            : baseQuery.OrderBy(lambda);

        return await orderedQuery.ToListAsync();
    }

    public static async Task<IReadOnlyList<Transaction>> GetByCategoryIdSortedAsync(
        this DbSet<Transaction> transactions,
        Guid categoryId,
        TransactionQueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Transaction), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var converted = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<Transaction, object>>(converted, parameter);

        // Apply the sorting to the query
        var baseQuery = transactions
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Include(x => x.Category)
            .Include(x => x.Currency)
            .Include(x => x.User)
            .ThenInclude(x => x.UserDetails)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Currency)
            .AsSplitQuery()
            .Where(x => x.CategoryId == categoryId && (x.Date >= queryParams.StartDate && x.Date <= queryParams.EndDate));

// Apply ordering
        var orderedQuery = queryParams.IsDescending
            ? baseQuery.OrderByDescending(lambda)
            : baseQuery.OrderBy(lambda);

        return await orderedQuery.ToListAsync();
    }
}