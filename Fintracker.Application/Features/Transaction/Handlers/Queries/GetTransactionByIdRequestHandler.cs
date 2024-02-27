using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Queries;

public class GetTransactionByIdRequestHandler : IRequestHandler<GetTransactionByIdRequest, TransactionBaseDTO>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionByIdRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<TransactionBaseDTO> Handle(GetTransactionByIdRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetAsync(request.Id);

        if (transaction is null)
            throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        return _mapper.Map<TransactionBaseDTO>(transaction);
    }
}