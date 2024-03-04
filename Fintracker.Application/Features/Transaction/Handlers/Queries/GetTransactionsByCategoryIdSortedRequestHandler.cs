using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Queries;

public class GetTransactionsByCategoryIdSortedRequestHandler : IRequestHandler<GetTransactionsByCategoryIdSortedRequest,
    IReadOnlyList<TransactionBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionsByCategoryIdSortedRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TransactionBaseDTO>> Handle(GetTransactionsByCategoryIdSortedRequest request,
        CancellationToken cancellationToken)
    {
        var transactions =
            await _unitOfWork.TransactionRepository.GetByCategoryIdSortedAsync(request.CategoryId, request.SortBy,
                request.IsDescending);

        //TODO validation logic if needed

        return _mapper.Map<List<TransactionBaseDTO>>(transactions);
    }
}