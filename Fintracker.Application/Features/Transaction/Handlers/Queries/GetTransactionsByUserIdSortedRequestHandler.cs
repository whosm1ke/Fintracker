using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Queries;

public class GetTransactionsByUserIdSortedRequestHandler : IRequestHandler<GetTransactionsByUserIdSortedRequest,
    IReadOnlyList<TransactionBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<string> _allowedSortColumns;

    public GetTransactionsByUserIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _allowedSortColumns = new()
        {
            nameof(Domain.Entities.Transaction.Label).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Note).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Id).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Amount).ToLowerInvariant()
        };
    }

    public async Task<IReadOnlyList<TransactionBaseDTO>> Handle(GetTransactionsByUserIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.Params.SortBy))
            throw new BadRequestException(
                new ExceptionDetails
                {
                    PropertyName = nameof(request.Params.SortBy),
                    ErrorMessage = $"Allowed values for sort by are {string.Join(", ", _allowedSortColumns)}"
                });

        var transactions =
            await _unitOfWork.TransactionRepository.GetByUserIdSortedAsync(request.UserId, request.Params);

        return _mapper.Map<List<TransactionBaseDTO>>(transactions);
    }
}