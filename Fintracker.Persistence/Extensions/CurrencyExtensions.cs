using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class CurrencyExtensions
{
    public static async Task<IReadOnlyList<Currency>> GetAllSortedAsync(
        this DbSet<Currency> categories,
        QueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Currency), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var lambda = Expression.Lambda<Func<Currency, object>>(property, parameter);

        // Apply the sorting to the query
        var query = queryParams.IsDescending
            ? categories
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .OrderByDescending(lambda)
            : categories
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }
}