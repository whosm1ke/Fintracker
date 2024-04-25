using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.Contracts.Infrastructure;

public interface ICurrencyConverter
{
    Task<ConvertCurrencyDTO?> Convert(string from, string to, decimal amount = 1);
    Task<List<ConvertCurrencyDTO?>> Convert(string from, IEnumerable<string> to, decimal amount = 1);
    Task<List<ConvertCurrencyDTO?>> Convert(IEnumerable<string> from, string to, decimal amount = 1);
    Task<List<ConvertCurrencyDTO?>> Convert(IEnumerable<string> from, string to, IEnumerable<decimal> amount);
}