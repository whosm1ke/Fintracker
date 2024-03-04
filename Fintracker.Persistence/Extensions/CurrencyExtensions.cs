using System.Linq.Expressions;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class CurrencyExtensions
{
    public static async Task<IReadOnlyList<Currency>> GetAllSortedAsync(
        this DbSet<Currency> categories,
        string sortBy,
        bool isDescending)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Currency), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, sortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Currency, object>>(property, parameter);

        // Apply the sorting to the query
        var query = isDescending
            ? categories.OrderByDescending(lambda)
            : categories.OrderBy(lambda);

        return await query.ToListAsync();
    }
}