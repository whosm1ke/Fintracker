using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class BudgetExtensions
{

    public static async Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(
        this IQueryable<Budget> budgets,
        Guid userId,
        BudgetQueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Budget), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var converted = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<Budget, object>>(converted, parameter);

        // Apply the sorting to the query
        var query = budgets
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Where(x => x.OwnerId == userId || x.Members.Any(mem => mem.Id == userId));

        if (queryParams.IsPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == queryParams.IsPublic.Value);
        }

        query = queryParams.IsDescending
            ? query.OrderByDescending(lambda)
            : query.OrderBy(lambda);

        return await query.ToListAsync();
    }


    public static async Task<IReadOnlyList<Budget>> GetByWalletIdSortedAsync(
        this IQueryable<Budget> budgets,
        Guid walletId,
        Guid userId,
        BudgetQueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Budget), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var converted = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<Budget, object>>(converted, parameter);

        // Apply the sorting to the query
        var query = budgets
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Where(x => (x.WalletId == walletId && x.OwnerId == userId) || x.Members.Any(mem => mem.Id == userId));

        if (queryParams.IsPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == queryParams.IsPublic.Value);
        }

        query = queryParams.IsDescending
            ? query.OrderByDescending(lambda)
            : query.OrderBy(lambda);

        return await query.ToListAsync();
    }
}