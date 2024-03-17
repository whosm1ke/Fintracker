using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Monobank.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Monobank.Handlers.Queries;

public class
    GetMonobankUserTransactionsRequestHandler : IRequestHandler<GetMonobankUserTransactionsRequest,
    IReadOnlyList<MonoTransactionDTO>>
{
    private readonly IMonobankService _monobankService;

    public GetMonobankUserTransactionsRequestHandler(IMonobankService monobankService)
    {
        _monobankService = monobankService;
    }

    public async Task<IReadOnlyList<MonoTransactionDTO>> Handle(GetMonobankUserTransactionsRequest request,
        CancellationToken cancellationToken)
    {
        var userInfo = await _monobankService.GetUserTransactions(request.Token, request.From,
            request.To, request.AccountId);

        return userInfo;
    }
}