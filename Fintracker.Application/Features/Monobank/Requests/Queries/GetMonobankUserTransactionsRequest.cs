using Fintracker.Application.DTO.Monobank;
using MediatR;

namespace Fintracker.Application.Features.Monobank.Requests.Queries;

public class GetMonobankUserTransactionsRequest : IRequest<IReadOnlyList<MonoTransactionDTO>>
{
    /// <summary>
    /// Should be provided from header
    /// </summary>
    public string Token { get; set; } = "0";

    public long From { get; set; }

    public long To { get; set; }

    public string AccountId { get; set; } = default!;
}