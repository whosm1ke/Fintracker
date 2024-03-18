using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Fintracker.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence.Repositories;

public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
{
   private readonly AppDbContext _db; 
    public CurrencyRepository(AppDbContext context): base(context)
    {
        _db = context;
    }

    public async Task<IReadOnlyList<Currency>> GetCurrenciesSorted(string sortBy, bool isDescending)
    {
        return await _db.Currencies.GetAllSortedAsync(sortBy, isDescending);
    }

    public async Task<Currency?> GetAsync(string symbol)
    {
        return await _db.Currencies
            .Where(x => x.Symbol == symbol)
            .FirstOrDefaultAsync();
    }

    public async Task<Currency?> GetAsync(int code)
    {
        return await _db.Currencies
            .Where(x => x.Code == code)
            .FirstOrDefaultAsync();
    }
}