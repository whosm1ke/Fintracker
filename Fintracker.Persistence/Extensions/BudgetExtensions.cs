using System.Linq.Expressions;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class BudgetExtensions
{
    public static async Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(
        this DbSet<Budget> budgets,
        Guid userId,
        string sortBy,
        bool isDescending)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Budget), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, sortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Budget, object>>(property, parameter);

        // Apply the sorting to the query
        var query = isDescending
            ? budgets
                .Include(x => x.Categories)
                .Include(x => x.Currency)
                .Where(x => x.UserId == userId)
                .OrderByDescending(lambda)
            : budgets
                .Include(x => x.Categories)
                .Include(x => x.Currency)
                .Where(x => x.UserId == userId)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }

    public static async Task<IReadOnlyList<Budget>> GetByWalletIdSortedAsync(
        this DbSet<Budget> budgets,
        Guid walletId,
        string sortBy,
        bool isDescending)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Budget), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, sortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Budget, object>>(property, parameter);

        // Apply the sorting to the query
        var query = isDescending
            ? budgets
                .Include(x => x.Categories)
                .Include(x => x.Currency)
                .Where(x => x.WalletId == walletId)
                .OrderByDescending(lambda)
            : budgets
                .Include(x => x.Categories)
                .Include(x => x.Currency)
                .Where(x => x.WalletId == walletId)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }
}