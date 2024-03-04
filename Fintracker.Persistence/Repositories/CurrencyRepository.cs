using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Fintracker.Persistence.Extensions;

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
}