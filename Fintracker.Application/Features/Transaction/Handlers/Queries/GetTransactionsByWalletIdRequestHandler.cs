using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Queries;

public class GetTransactionsByWalletIdRequestHandler: IRequestHandler<GetTransactionsByWalletIdRequest, IReadOnlyList<TransactionBaseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionsByWalletIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<IReadOnlyList<TransactionBaseDTO>> Handle(GetTransactionsByWalletIdRequest request, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.TransactionRepository.GetByWalletIdAsync(request.WalletId);
        
        //TODO validation logic if needed

        return _mapper.Map<List<TransactionBaseDTO>>(transactions);
    }
}