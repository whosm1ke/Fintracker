using System.Linq.Expressions;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class TransactionExtensions
{
    public static async Task<IReadOnlyList<Transaction>> GetByUserIdSortedAsync(
        this DbSet<Transaction> transactions,
        Guid userId,
        string sortBy,
        bool isDescending)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Transaction), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, sortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Transaction, object>>(property, parameter);

        // Apply the sorting to the query
        var query = isDescending
            ? transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .Where(x => x.UserId == userId)
                .OrderByDescending(lambda)
            : transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .Where(x => x.UserId == userId)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }

    public static async Task<IReadOnlyList<Transaction>> GetByWalletIdSortedAsync(
        this DbSet<Transaction> transactions,
        Guid walletId,
        string sortBy,
        bool isDescending)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Transaction), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, sortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Transaction, object>>(property, parameter);

        // Apply the sorting to the query
        var query = isDescending
            ? transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .Where(x => x.WalletId == walletId)
                .OrderByDescending(lambda)
            : transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .Where(x => x.WalletId == walletId)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }

    public static async Task<IReadOnlyList<Transaction>> GetByCategoryIdSortedAsync(
        this DbSet<Transaction> transactions,
        Guid categoryId,
        string sortBy,
        bool isDescending)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Transaction), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, sortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Transaction, object>>(property, parameter);

        // Apply the sorting to the query
        var query = isDescending
            ? transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(lambda)
            : transactions
                .Include(x => x.Category)
                .Include(x => x.Currency)
                .Where(x => x.CategoryId == categoryId)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }
}