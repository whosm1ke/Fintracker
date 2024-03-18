using Fintracker.Application.Models;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface ICurrencyRepository : IGenericRepository<Currency>
{
    Task<IReadOnlyList<Currency>> GetCurrenciesSorted(QueryParams queryParams);

    Task<Currency?> GetAsync(string symbol);
    Task<Currency?> GetAsync(int code);
}