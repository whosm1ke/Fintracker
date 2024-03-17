using Fintracker.Application.DTO.Monobank;

namespace Fintracker.Application.Contracts.Infrastructure;

public interface IMonobankService
{
    Task<MonobankUserInfoDTO?> GetUserFullInfo(string token);
    Task<IReadOnlyList<JarDTO>> GetUserJars(string token);
    Task<IReadOnlyList<AccountDTO>> GetUserAccounts(string token);
    Task<IReadOnlyList<MonoTransactionDTO>> GetUserTransactions(string token, long from, long to, string accountId);
}