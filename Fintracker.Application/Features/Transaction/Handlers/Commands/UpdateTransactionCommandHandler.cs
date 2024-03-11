using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.Transaction.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class
    UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand,
    UpdateCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCommandResponse<TransactionBaseDTO>> Handle(UpdateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<TransactionBaseDTO>();
        var validator = new UpdateTransactionDtoValidator(_unitOfWork);
        var validationResult = await validator.ValidateAsync(request.Transaction);

        var transaction = await _unitOfWork.TransactionRepository.GetTransactionWithWalletAsync(request.Transaction.Id);

        if (transaction is null)
            throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Transaction.Id);

        if (validationResult.IsValid)
        {
            var oldTransaction = _mapper.Map<TransactionBaseDTO>(transaction);
            _mapper.Map(request.Transaction, transaction);
            var newTransaction = _mapper.Map<TransactionBaseDTO>(transaction);

            await UpdateBudgetBalance(request.Transaction.CategoryId, request.Transaction.Amount,
                oldTransaction.Amount);

            await UpdateWalletBalance(transaction.Wallet, request.Transaction.Amount,
                oldTransaction.Amount);

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
            throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        return response;
    }

    private async Task UpdateWalletBalance(Domain.Entities.Wallet wallet, decimal newAmount, decimal oldAmount)
    {
        decimal difference = newAmount - oldAmount;

        if (difference > 0)
            wallet.Balance += difference;
        else
            wallet.Balance -= difference;
    }

    private async Task UpdateBudgetBalance(Guid categoryId, decimal newAmount, decimal oldAmount)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(categoryId);
        decimal difference = newAmount - oldAmount;

        foreach (var budget in budgets)
        {
            if (difference > 0)
                budget.Balance += difference;
            else
                budget.Balance -= difference;
        }
    }
}