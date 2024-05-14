using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Queries;

public class GetTransactionsByWalletIdSortedRequestHandler : IRequestHandler<GetTransactionsByWalletIdSortedRequest,
    IReadOnlyList<TransactionBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<string> _allowedSortColumns;

    public GetTransactionsByWalletIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _allowedSortColumns = new()
        {
            nameof(Domain.Entities.Transaction.Label).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Note).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Id).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Amount).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.AmountInWalletCurrency).ToLowerInvariant()
        };
    }

    public async Task<IReadOnlyList<TransactionBaseDTO>> Handle(GetTransactionsByWalletIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.Params.SortBy!))
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = nameof(request.Params.SortBy),
                ErrorMessage = $"Allowed values for sort by are {string.Join(", ", _allowedSortColumns)}"
            });

        var transactions =
            await _unitOfWork.TransactionRepository.GetByWalletIdSortedAsync(request.WalletId, request.Params);

        var groupedTransactions = transactions
            .GroupBy(t => t.Date.Date)
            .Select(g => g.OrderByDescending(t => t.Date)
                                                    .Take(request.Params.TransactionsPerDate ?? int.MaxValue))
            .SelectMany(g => g)
            .ToList();

        var transactionDTOs = _mapper.Map<List<TransactionBaseDTO>>(groupedTransactions);

        return transactionDTOs;
    }
}