using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, DeleteCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteCommandResponse<TransactionBaseDTO>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var response = new DeleteCommandResponse<TransactionBaseDTO>();
        
        var transaction = await _unitOfWork.TransactionRepository.GetTransactionWithWalletAsync(request.Id);

        if (transaction is null)
            throw new NotFoundException(nameof(Domain.Entities.Transaction), request.Id);

        var deletedObj = _mapper.Map<TransactionBaseDTO>(transaction);

        await IncreaseWalletBalance(transaction.Wallet, transaction.Amount);
        await IncreaseBudgetBalance(transaction.CategoryId, transaction.Amount);
        
        await _unitOfWork.TransactionRepository.DeleteAsync(transaction);

        response.Success = true;
        response.Message = "Deleted successfully";
        response.DeletedObj = deletedObj;
        await _unitOfWork.SaveAsync();
        
        return response;
    }

    private async Task IncreaseWalletBalance(Domain.Entities.Wallet wallet, decimal amount)
    {
        wallet.Balance += amount;
    }
    
    private async Task IncreaseBudgetBalance(Guid categoryId, decimal amount)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(categoryId);

        foreach (var budget in budgets)
        {
            budget.Balance += amount;
        }
    }
}