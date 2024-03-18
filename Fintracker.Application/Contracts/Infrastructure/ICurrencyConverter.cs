using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.Contracts.Infrastructure;

public interface ICurrencyConverter
{
    Task<ConvertCurrencyDTO> Convert(string from, string to, decimal amount = 1m);
}