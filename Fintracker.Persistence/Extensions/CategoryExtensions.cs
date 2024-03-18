using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class CategoryExtensions
{
    public static async Task<IReadOnlyList<Category>> GetAllSortedAsync(
        this DbSet<Category> categories,
        Guid userId,
        QueryParams queryParams)
    {
        // Create a parameter expression for the entity type
        var parameter = Expression.Parameter(typeof(Category), "x");

        // Create a property access expression for the specified sort column
        var property = Expression.Property(parameter, queryParams.SortBy);

        // Create a lambda expression for the OrderBy method
        var converted = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<Category, object>>(converted, parameter);

        // Apply the sorting to the query
        var query = queryParams.IsDescending
            ? categories
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Where(c => c.UserId == userId)
                .OrderByDescending(lambda)
            : categories
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Where(c => c.UserId == userId)
                .OrderBy(lambda);

        return await query.ToListAsync();
    }
}