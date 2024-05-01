using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class TransactionExtensions
{
    public static async Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(
        this IQueryable<Transaction> transactions,
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
            .Where(x => x.UserId == userId && x.Date.Date >= queryParams.StartDate.Date &&
                        x.Date.Date <= queryParams.EndDate.Date);

// Apply ordering
        var orderedQuery = queryParams.IsDescending
            ? baseQuery.OrderByDescending(lambda)
            : baseQuery.OrderBy(lambda);

        return await orderedQuery.ToListAsync();
    }

    public static async Task<IReadOnlyList<Transaction>> GetByWalletIdSortedAsync(
        this IQueryable<Transaction> transactions,
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
            .Where(x => x.WalletId == walletId && x.Date.Date >= queryParams.StartDate.Date &&
                        x.Date.Date <= queryParams.EndDate.Date);

// Apply ordering
        var orderedQuery = queryParams.IsDescending
            ? baseQuery.OrderByDescending(lambda)
            : baseQuery.OrderBy(lambda);

        return await orderedQuery.ToListAsync();
    }

    public static async Task<IReadOnlyList<Transaction>> GetByCategoryIdSortedAsync(
        this IQueryable<Transaction> transactions, Guid userId,
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
            .Where(x => x.CategoryId == categoryId && x.UserId == userId && x.Date.Date >= queryParams.StartDate.Date &&
                        x.Date.Date <= queryParams.EndDate.Date);

// Apply ordering
        var orderedQuery = queryParams.IsDescending
            ? baseQuery.OrderByDescending(lambda)
            : baseQuery.OrderBy(lambda);

        return await orderedQuery.ToListAsync();
    }
}