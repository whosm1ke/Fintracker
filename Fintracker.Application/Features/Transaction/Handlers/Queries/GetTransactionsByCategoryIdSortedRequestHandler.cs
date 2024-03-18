using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Queries;

public class GetTransactionsByCategoryIdSortedRequestHandler : IRequestHandler<GetTransactionsByCategoryIdSortedRequest,
    IReadOnlyList<TransactionBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<string> _allowedSortColumns;

    public GetTransactionsByCategoryIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;

        _allowedSortColumns = new()
        {
            nameof(Domain.Entities.Transaction.Label).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Note).ToLowerInvariant(),
            nameof(Domain.Entities.Transaction.Amount).ToLowerInvariant()
        };
    }

    public async Task<IReadOnlyList<TransactionBaseDTO>> Handle(GetTransactionsByCategoryIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.Params.SortBy))
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = nameof(request.Params.SortBy),
                ErrorMessage = $"Allowed values for sort by are {string.Join(", ", _allowedSortColumns)}"
            });

        var transactions =
            await _unitOfWork.TransactionRepository.GetByCategoryIdSortedAsync(request.CategoryId, request.Params);


        return _mapper.Map<List<TransactionBaseDTO>>(transactions);
    }
}