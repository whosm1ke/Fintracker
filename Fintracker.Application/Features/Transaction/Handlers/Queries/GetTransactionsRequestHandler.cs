using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Queries;

public class GetTransactionsRequestHandler : IRequestHandler<GetTransactionsRequest, IReadOnlyList<TransactionBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionsRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TransactionBaseDTO>> Handle(GetTransactionsRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetAllAsync();


        return _mapper.Map<List<TransactionBaseDTO>>(transaction);
    }
}