using System.Net;
using System.Net.Http.Json;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Fintracker.Infrastructure.CurrenciesAPI;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _cfg;

    public CurrencyConverter(IHttpClientFactory httpClientFactory, IConfiguration cfg)
    {
        _httpClientFactory = httpClientFactory;
        _cfg = cfg;
    }

    public async Task<ConvertCurrencyDTO> Convert(string from, string to, decimal amount = 1)
    {
        using (var client = _httpClientFactory.CreateClient("CurrencyBeacon"))
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress!, 
                    $"v1/convert?api_key={_cfg["CurrencyBeacon:API_KEY"]}&from={from}&to={to}&amount={amount}")
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
}