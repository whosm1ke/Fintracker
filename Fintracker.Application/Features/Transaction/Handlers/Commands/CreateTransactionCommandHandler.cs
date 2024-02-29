using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.Transaction.Validators;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CreateCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCommandResponse<TransactionBaseDTO>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateCommandResponse<TransactionBaseDTO>();
        var validator = new CreateTransactionDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.Transaction);

        if (validationResult.IsValid)
        {
            var transaction = _mapper.Map<Domain.Entities.Transaction>(request.Transaction);
            await _unitOfWork.TransactionRepository.AddAsync(transaction);

            var createdObj = _mapper.Map<TransactionBaseDTO>(transaction);

            response.Success = true;
            response.Message = "Created successfully";
            response.Id = createdObj.Id;
            response.CreatedObject = createdObj;

            await _unitOfWork.SaveAsync();
        }
        else
        {
            response.Success = false;
            response.Message = "Creation failed";
            response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        }
        
        return response;
    }
}