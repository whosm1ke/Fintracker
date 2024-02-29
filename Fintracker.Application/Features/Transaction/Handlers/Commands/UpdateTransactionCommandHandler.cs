using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.Transaction.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, UpdateCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateCommandResponse<TransactionBaseDTO>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<TransactionBaseDTO>();
        var validator = new UpdateTransactionDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.Transaction);

        if (validationResult.IsValid)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetAsync(request.Transaction.Id);

            if (transaction is null)
                throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Transaction.Id);

            var oldTransaction = _mapper.Map<TransactionBaseDTO>(transaction);
            _mapper.Map(request.Transaction, transaction);
            var newTransaction = _mapper.Map<TransactionBaseDTO>(transaction);
            await _unitOfWork.TransactionRepository.UpdateAsync(transaction);

            response.Success = true;
            response.Message = "Updated successfully";
            response.Old = oldTransaction;
            response.New = newTransaction;
            response.Id = request.Transaction.Id;
            
            await _unitOfWork.SaveAsync();
        }
        else
        {
            response.Success = false;
            response.Message = "Update failed";
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        }

        return response;
    }
}