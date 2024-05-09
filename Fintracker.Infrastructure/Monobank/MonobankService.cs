using System.Net;
using System.Net.Http.Json;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.Exceptions;
using Fintracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fintracker.Infrastructure.Monobank;

public class MonobankService : IMonobankService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<User> _userManager;

    public MonobankService(IHttpClientFactory httpClientFactory, UserManager<User> userManager)
    {
        _httpClientFactory = httpClientFactory;
        _userManager = userManager;
    }

    public async Task SetMonobankTokenAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "Invalid email",
                PropertyName = nameof(email)
            }, nameof(User));

        var res = await _userManager.SetAuthenticationTokenAsync(user, "Monobank",
            "Access_Token", token);

        if (!res.Succeeded)
            throw new BadRequestException(res.Errors.Select(x => new ExceptionDetails
            {
                ErrorMessage = x.Description,
                PropertyName = ""
            }).ToList());
    }

    public async Task<string?> GetMonobankTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "Invalid email",
                PropertyName = nameof(email)
            }, nameof(User));

        return await _userManager.GetAuthenticationTokenAsync(user, "Monobank",
            "Access_Token");
    }

    public async Task<MonobankUserInfoDTO?> GetUserFullInfo(string xToken)
    {
        using (var client = _httpClientFactory.CreateClient("MonobankClient"))
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress!, "personal/client-info")
            };
            requestMessage.Headers.Add("X-Token", xToken);

            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

            if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenException(new ExceptionDetails
                {
                    ErrorMessage = "Unknown 'X-Token'",
                    PropertyName = nameof(xToken)
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

 

    public async Task<decimal> GetAccountBalance(string token, string accountId)
    {
        var accounts = new List<IAccountBaseDto>();
        var allAccounts = accounts
            .Union(await GetUserAccounts(token));

        var accountToInspect = allAccounts
            .FirstOrDefault(x => x.Id == accountId);

        if (accountToInspect is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "Account or jar was not fount by id",
                PropertyName = nameof(accountId)
            }, "Account or Jar");

        return Convert.ToDecimal(accountToInspect.Balance);
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
                throw new ForbiddenException(new ExceptionDetails
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
            
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                throw new BadRequestException(new ExceptionDetails
                {
                    ErrorMessage = "Please select a period between one month",
                    PropertyName = "Error"
                });

            IReadOnlyList<MonoTransactionDTO> transactions =
                (await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<MonoTransactionDTO>>())!;

            return transactions;
        }
    }
}