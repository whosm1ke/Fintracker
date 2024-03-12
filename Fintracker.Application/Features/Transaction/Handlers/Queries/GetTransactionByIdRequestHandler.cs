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
        var transaction = await _unitOfWork.TransactionRepository.GetTransactionAsync(request.Id);

        if (transaction is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            },nameof(Domain.Entities.Transaction));

        return _mapper.Map<TransactionBaseDTO>(transaction);
    }
}