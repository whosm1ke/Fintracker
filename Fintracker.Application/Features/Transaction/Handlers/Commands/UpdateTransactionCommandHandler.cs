using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
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

        var transaction = await _unitOfWork.TransactionRepository.GetTransactionWithWalletAsync(request.Transaction.Id);

        if (transaction is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Transaction.Id}]",
                PropertyName = nameof(request.Transaction.Id)
            }, nameof(Domain.Entities.Transaction));

        if (transaction.IsBankTransaction && transaction.Amount.CompareTo(request.Transaction.Amount) != 0)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Can not change bank transaction amount",
                PropertyName = nameof(request.Transaction.Amount)
            });
        
        var oldObject = _mapper.Map<TransactionBaseDTO>(transaction);
        _mapper.Map(request.Transaction, transaction);
        
        if (!transaction.IsBankTransaction)
        {
            await UpdateBudgetBalance(request.Transaction.CategoryId, request.Transaction.Amount, oldObject.Amount,
                oldObject.UserId);
            UpdateWalletBalance(transaction.Wallet, request.Transaction.Amount, oldObject.Amount);
            _unitOfWork.TransactionRepository.Update(transaction);
        }

        await _unitOfWork.SaveAsync();
        var newObject = _mapper.Map<TransactionBaseDTO>(transaction);

        response.Success = true;
        response.Message = "Updated successfully";
        response.Old = oldObject;
        response.New = newObject;
        response.Id = request.Transaction.Id;


        return response;
    }

    private void UpdateWalletBalance(Domain.Entities.Wallet wallet, decimal newAmount, decimal oldAmount)
    {
        decimal difference = newAmount - oldAmount;

        if (difference > 0)
            wallet.Balance += difference;
        else
            wallet.Balance -= difference;
    }

    private async Task UpdateBudgetBalance(Guid categoryId, decimal newAmount, decimal oldAmount, Guid userId)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(categoryId);
        decimal difference = newAmount - oldAmount;

        foreach (var budget in budgets)
        {
            if (budget.IsPublic || budget.UserId == userId)
            {
                if (difference > 0)
                    budget.Balance += difference;
                else
                    budget.Balance -= difference;
            }
        }
    }
}