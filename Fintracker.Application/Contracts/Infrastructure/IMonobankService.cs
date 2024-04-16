using Fintracker.Application.DTO.Monobank;

namespace Fintracker.Application.Contracts.Infrastructure;

public interface IMonobankService
{
    Task SetMonobankTokenAsync(string email, string token);
    Task<string?> GetMonobankTokenAsync(string email);
    Task<MonobankUserInfoDTO?> GetUserFullInfo(string xToken);

    Task<decimal> GetAccountBalance(string token, string accountId);
    Task<IReadOnlyList<AccountDTO>> GetUserAccounts(string token); 
    Task<IReadOnlyList<MonoTransactionDTO>> GetUserTransactions(string token, long from, long to, string accountId);
    
}