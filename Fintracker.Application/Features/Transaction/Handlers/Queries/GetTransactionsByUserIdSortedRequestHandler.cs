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
            nameof(Domain.Entities.Transaction.Label),
            nameof(Domain.Entities.Transaction.Note),
            nameof(Domain.Entities.Transaction.Amount),
            nameof(Domain.Entities.Transaction.Category),
            nameof(Domain.Entities.Transaction.User)
        };
    }

    public async Task<IReadOnlyList<TransactionBaseDTO>> Handle(GetTransactionsByUserIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        if (!_allowedSortColumns.Contains(request.SortBy))
            throw new BadRequestException(
                $"Invalid sortBy parameter. Allowed values {string.Join(',', _allowedSortColumns)}");

        var transactions =
            await _unitOfWork.TransactionRepository.GetByUserIdSortedAsync(request.UserId, request.SortBy,
                request.IsDescending);

        return _mapper.Map<List<TransactionBaseDTO>>(transactions);
    }
}