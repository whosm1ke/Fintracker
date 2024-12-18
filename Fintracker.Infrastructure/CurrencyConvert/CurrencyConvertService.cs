using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Fintracker.Infrastructure.CurrencyConvert;

public class CurrencyConvertService : ICurrencyConverter
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _cfg;

    public CurrencyConvertService(IHttpClientFactory httpClientFactory, IConfiguration cfg)
    {
        _httpClientFactory = httpClientFactory;
        _cfg = cfg;
    }

    public async Task<ConvertCurrencyDTO?> Convert(string from, string to, decimal amount = 1)
    {
        if (from == to)
            return new ConvertCurrencyDTO
            {
                Amount = amount,
                From = from,
                To = to,
                Value = amount
            };

        using (var client = _httpClientFactory.CreateClient("CurrencyBeacon"))
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress!,
                    $"v1/convert?api_key={_cfg["CurrencyBeacon:API_KEY"]}&from={from}&to={to}&amount={amount.ToString(CultureInfo.InvariantCulture)}")
            };

            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

            if (responseMessage.StatusCode == HttpStatusCode.InternalServerError)
                throw new BadRequestException(new ExceptionDetails
                {
                    ErrorMessage = "Something went wrong. Check currencies symbols",
                    PropertyName = nameof(from) + ", " + nameof(to)
                });

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException(new ExceptionDetails
                {
                    ErrorMessage = $"Currency {from} or {to} was not found",
                    PropertyName = nameof(from) + ", " + nameof(to)
                }, "Currency");

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Invalid API key for CurrencyBeacon");

            ConvertCurrencyDTO currency = (await responseMessage.Content.ReadFromJsonAsync<ConvertCurrencyDTO>())!;

            return currency;
        }
    }

    public async Task<List<ConvertCurrencyDTO?>> Convert(string from, IEnumerable<string> to, decimal amount = 1)
    {
        var uniqueToSymbols = to.Distinct();
        List<ConvertCurrencyDTO?> results = new();
        foreach (var toCurrency in uniqueToSymbols)
        {
            var convertredCurrency = await Convert(from, toCurrency, amount);
            results.Add(convertredCurrency);
        }

        return results;
    }


    public async Task<List<ConvertCurrencyDTO?>> Convert(IEnumerable<string> from, string to, decimal amount = 1)
    {
        var uniqueFromSymbols = from.Distinct();
        List<ConvertCurrencyDTO?> results = new();
        foreach (var fromCurrency in uniqueFromSymbols)
        {
            var convertredCurrency = await Convert(fromCurrency, to, amount);
            results.Add(convertredCurrency);
        }

        return results;
    }

    public async Task<List<ConvertCurrencyDTO?>> Convert(IEnumerable<string> from, string to,
        IEnumerable<decimal> amounts)
    {
        var fromList = from.ToList();
        var amountsList = amounts.ToList();
        if (fromList.Count != amountsList.Count)
            throw new ArgumentException($"Length of {nameof(from)} is not equal to length of {nameof(amounts)}");

        var uniqueFromSymbols = fromList.Distinct();

        // Create a dictionary to store the conversion rate for each unique currency symbol
        Dictionary<string, ConvertCurrencyDTO?> conversionRates = new();

        foreach (var fromCurrency in uniqueFromSymbols)
        {
            var convertedCurrency = await Convert(fromCurrency, to, 1);
            conversionRates[fromCurrency] = convertedCurrency;
        }

        // Create a list to store the final results
        List<ConvertCurrencyDTO?> results = new();

        for (int i = 0; i < fromList.Count; i++)
        {
            var fromCurrency = fromList[i];
            var amount = amountsList[i];

            // Get the conversion rate for the current currency symbol
            var conversionRate = conversionRates[fromCurrency];

            // Create a new ConvertCurrencyDTO with the converted amount
            var result = new ConvertCurrencyDTO
            {
                From = fromCurrency,
                To = to,
                Amount = amount,
                Value = conversionRate?.Value * amount ?? amount
            };

            results.Add(result);
        }

        return results;
    }
}


// public async Task<IReadOnlyList<Transaction>> GetByWalletIdForBudgetCreationAsync(Guid walletId, DateTime budgetStart, DateTime budgetEnd)
// {
//     return await _db.Transactions
//         .Include(x => x.Category)
//         .Include(x => x.Currency)
//         .Where(x => x.WalletId == walletId && x.Date >= budgetStart && x.Date <= budgetEnd)
//         .ToListAsync();
// }
//
// private async Task PopulateNewBudget(Guid walletId, DateTime budgetStart, DateTime budgetEnd)
// {
//     var transactionsPerBudget =
//         _unitOfWork.TransactionRepository.GetByWalletIdForBudgetCreationAsync(walletId, budgetStart, budgetEnd);
//         
//         
// }
//
// І так, щойно придумав нову логіку при створенні бюджету. Наразі, я не правильно створюю бюджет, оскільки він створюється і тільки після додавання нових транзакцій починає працювати, але треба, щоб він починав рахувати одразу після створення.
//
//     Тобто такий сценарій. У користувача є 5 транзакцій з 1.04 по 5.04, по транзакції на день. Він створює бюджет з 2.04 по 4.04 і бюджет вже має включати розрахунок по транзакціях з 2.04 по 4.04.
//
//     Я там дописав новий метод, він йде з інтерфейсу. Тобі треба дописати метод  PopulateNewBudget (або додати до його параметрів, щось що потрбіно), щоб він:
// 1. Конвертував валюти транзакцій у валюту бюджету. Тобто наприклад ці три транзакції мають три різні валюти, які відрізняються від валюти бюджету. Щоб розрахувати вже витрачену суму (тотал спент), треба відняти суму транзакцій у 