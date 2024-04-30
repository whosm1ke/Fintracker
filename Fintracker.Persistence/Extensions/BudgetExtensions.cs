using System.Linq.Expressions;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Extensions;

public static class BudgetExtensions
{
    public static async Task<IReadOnlyList<Budget>> GetByOwnerIdSortedAsync(
        this DbSet<Budget> budgets,
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
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Category)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.Owner)
            .ThenInclude(x => x.UserDetails)
            .AsSplitQuery()
            .Where(x => x.OwnerId == userId);

        if (queryParams.IsPublic.HasValue)
        {
            query = query.Where(x => x.IsPublic == queryParams.IsPublic.Value);
        }

        query = queryParams.IsDescending
            ? query.OrderByDescending(lambda)
            : query.OrderBy(lambda);

        return await query.ToListAsync();
    }

    public static async Task<IReadOnlyList<Budget>> GetByUserIdSortedAsync(
    this DbSet<User> users,
    Guid userId,
    BudgetQueryParams queryParams)
{
    // Create a parameter expression for the entity type
    var parameter = Expression.Parameter(typeof(Budget), "x");

    // Create a property access expression for the specified sort column
    var property = Expression.Property(parameter, queryParams.SortBy);
    var converted = Expression.Convert(property, property.Type);
    var lambda = Expression.Lambda(converted, parameter);

    var lambdaFunc = lambda.Compile();

    var user = await users
        .Include(u => u.OwnedBudgets)
            .ThenInclude(b => b.Categories)
        .Include(u => u.OwnedBudgets)
            .ThenInclude(b => b.Currency)
        .Include(u => u.OwnedBudgets)
            .ThenInclude(b => b.Transactions)
                .ThenInclude(t => t.Currency)
        .Include(u => u.OwnedBudgets)
            .ThenInclude(b => b.Transactions)
                .ThenInclude(t => t.Category)
        .Include(u => u.OwnedBudgets)
            .ThenInclude(b => b.Wallet)
                .ThenInclude(t => t.Currency)
        .Include(u => u.MemberBudgets)
            .ThenInclude(b => b.Categories)
        .Include(u => u.MemberBudgets)
            .ThenInclude(b => b.Currency)
        .Include(u => u.MemberBudgets)
            .ThenInclude(b => b.Transactions)
                .ThenInclude(t => t.Currency)
        .Include(u => u.MemberBudgets)
            .ThenInclude(b => b.Transactions)
                .ThenInclude(t => t.Category)
        .Include(u => u.MemberBudgets)
            .ThenInclude(b => b.Wallet)
                .ThenInclude(t => t.Currency)
        .AsSplitQuery()
        .FirstOrDefaultAsync(x => x.Id == userId);

    if (user is null) return new List<Budget>();

    var ownedBudgets = user.OwnedBudgets.AsQueryable();
    var memberBudgets = user.MemberBudgets.AsQueryable();

    if (queryParams.IsPublic.HasValue)
    {
        ownedBudgets = ownedBudgets.Where(x => x.IsPublic == queryParams.IsPublic.Value);
        memberBudgets = memberBudgets.Where(x => x.IsPublic == queryParams.IsPublic.Value);
    }

    var budgets = ownedBudgets.Union(memberBudgets).DistinctBy(b => b.Id).AsQueryable();

    budgets = queryParams.IsDescending
        ? budgets.AsEnumerable().OrderByDescending((Func<Budget, IComparable>)lambdaFunc).AsQueryable()
        : budgets.AsEnumerable().OrderBy((Func<Budget, IComparable>)lambdaFunc).AsQueryable();

    return budgets.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                  .Take(queryParams.PageSize)
                  .ToList();
    }


    public static async Task<IReadOnlyList<Budget>> GetByWalletIdSortedAsync(
        this DbSet<Budget> budgets,
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
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Category)
            .Include(x => x.Transactions)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Categories)
            .Include(x => x.Currency)
            .Include(x => x.Wallet)
            .ThenInclude(x => x.Owner)
            .Include(x => x.Owner)
            .ThenInclude(x => x.UserDetails)
            .Include(b => b.Members)
            .ThenInclude(x => x.UserDetails)
            .AsSplitQuery()
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