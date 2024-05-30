using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.DTO.Currency;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockCurrencyConverter
{
    public static Mock<ICurrencyConverter> GetCurrencyConverter()
    {
        var mock = new Mock<ICurrencyConverter>();

        // Convert(string from, string to, decimal amount = 1)
        mock.Setup(converter => converter.Convert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
            .ReturnsAsync((string from, string to, decimal amount) =>
                new ConvertCurrencyDTO
                {
                    From = from,
                    To = to,
                    Amount = amount,
                    Value = amount * GetExchangeRate(from, to) // Simulate a conversion
                });

        // Convert(string from, IEnumerable<string> to, decimal amount = 1)
        mock.Setup(converter => converter.Convert(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<decimal>()))!
            .ReturnsAsync((string from, IEnumerable<string> toList, decimal amount) =>
                toList.Select(to => new ConvertCurrencyDTO
                {
                    From = from,
                    To = to,
                    Amount = amount,
                    Value = amount * GetExchangeRate(from, to)
                }).ToList());

        // Convert(IEnumerable<string> from, string to, decimal amount = 1)
        mock.Setup(converter => converter.Convert(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<decimal>()))!
            .ReturnsAsync((IEnumerable<string> fromList, string to, decimal amount) =>
                fromList.Select(from => new ConvertCurrencyDTO
                {
                    From = from,
                    To = to,
                    Amount = amount,
                    Value = amount * GetExchangeRate(from, to)
                }).ToList());

        // Convert(IEnumerable<string> from, string to, IEnumerable<decimal> amount)
        mock.Setup(converter => converter.Convert(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<IEnumerable<decimal>>()))!
            .ReturnsAsync((IEnumerable<string> fromList, string to, IEnumerable<decimal> amounts) =>
                fromList.Zip(amounts, (from, amount) => new ConvertCurrencyDTO
                {
                    From = from,
                    To = to,
                    Amount = amount,
                    Value = amount * GetExchangeRate(from, to)
                }).ToList());

        return mock;
    }

    // Helper method to simulate exchange rates (replace with your actual logic)
    private static decimal GetExchangeRate(string from, string to)
    {
        
        return 1.0m; // Default 1:1 exchange rate
    }
}