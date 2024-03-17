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
        var token = await _monobankService.GetMonobankTokenAsync(request.Email);

        if (token is null)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Monobank token was not provided",
                PropertyName = nameof(token)
            });

        request.Configuration.To ??= (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;

        var monoTransactions = await _monobankService.GetUserTransactions(token, request.Configuration.From,
            request.Configuration.To.Value, request.Configuration.AccountId);

        return monoTransactions;
    }
}