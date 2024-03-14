using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Common;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Fintracker.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity<Guid>
{
    private readonly IDictionary<string, string> _columnsNames = new Dictionary<string, string>()
    {
        { nameof(User), "AspNetUsers" },
        { nameof(Budget), "Budgets" },
        { nameof(Category), "Categories" },
        { nameof(Currency), "Currencies" },
        { nameof(Transaction), "Transactions" },
        { nameof(Wallet), "Wallets" },
    };
    private readonly AppDbContext _db;
    public GenericRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<T?> GetAsync(Guid id)
    {
        return await _db.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetAsyncNoTracking(Guid id)
    {
        return await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<T?>> GetAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T?>> GetAllAsyncNoTracking()
    {
        return await _db.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T> AddAsync(T item)
    {
        await _db.AddAsync(item);
        return item;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var modelName = typeof(T).Name;
        if (!_columnsNames.TryGetValue(modelName, out var tableName))
        {
            throw new Exception($"No table name mapping found for model {modelName}.");
        }
        var query = $"SELECT EXISTS (SELECT 1 FROM \"public\".\"{tableName}\" WHERE \"{tableName}\".\"Id\" = @p0)";

        using (var command = _db.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = query;
            command.Parameters.Add(new NpgsqlParameter("@p0", id));

            await _db.Database.OpenConnectionAsync();

            var result = await command.ExecuteScalarAsync();

            var res = Convert.ToBoolean(result);
            return res;
        }
    }



    public void Update(T item)
    {
        _db.Entry(item).State = EntityState.Modified;
    }

    public void Delete(T item)
    {
        _db.Set<T>().Remove(item);
    }
}