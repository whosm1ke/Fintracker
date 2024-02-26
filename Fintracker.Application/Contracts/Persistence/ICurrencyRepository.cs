﻿using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Persistence;

public interface ICurrencyRepository : IGenericRepository<Currency>
{
    Task<IReadOnlyList<Currency>> GetCurrenciesSorted(string sortBy);
}