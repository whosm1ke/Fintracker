using System.Net;
using System.Net.Http.Json;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.Exceptions;

namespace Fintracker.Infrastructure.Monobank;

public class MonobankService : IMonobankService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MonobankService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MonobankUserInfoDTO?> GetUserFullInfo(string token)
    {
        using (var client = _httpClientFactory.CreateClient("MonobankClient"))
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress!, "personal/client-info")
            };
            requestMessage.Headers.Add("X-Token", token);

            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

            if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbidenException(new ExceptionDetails
                {
                    ErrorMessage = "Unknown 'X-Token'",
                    PropertyName = nameof(token)
                });

            if (responseMessage.StatusCode == HttpStatusCode.TooManyRequests)
                throw new BadRequestException(new ExceptionDetails
                {
                    ErrorMessage = "Too many requests",
                    PropertyName = "Error"
                });

            MonobankUserInfoDTO? info = await responseMessage.Content.ReadFromJsonAsync<MonobankUserInfoDTO>();

            return info;
        }
    }

    public async Task<IReadOnlyList<JarDTO>> GetUserJars(string token)
    {
        MonobankUserInfoDTO? userInfo = await GetUserFullInfo(token);


        return userInfo!.Jars.ToList();
    }

    public async Task<IReadOnlyList<AccountDTO>> GetUserAccounts(string token)
    {
        MonobankUserInfoDTO? userInfo = await GetUserFullInfo(token);

        return userInfo!.Accounts.ToList();
    }

    public async Task<IReadOnlyList<MonoTransactionDTO>> GetUserTransactions(string token, long from, long to,
        string accountId)
    {
        using (var client = _httpClientFactory.CreateClient("MonobankClient"))
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress!, $"personal/statement/{accountId}/{from}/{to}")
            };
            requestMessage.Headers.Add("X-Token", token);

            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

            if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbidenException(new ExceptionDetails
                {
                    ErrorMessage = "Unknown 'X-Token'",
                    PropertyName = nameof(token)
                });

            if (responseMessage.StatusCode == HttpStatusCode.TooManyRequests)
                throw new BadRequestException(new ExceptionDetails
                {
                    ErrorMessage = "Too many requests",
                    PropertyName = "Error"
                });
            
            IReadOnlyList<MonoTransactionDTO> transactions =
                (await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<MonoTransactionDTO>>())!;

            return transactions;
        }
    }
}