﻿using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class
    DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand,
    DeleteCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteCommandResponse<TransactionBaseDTO>> Handle(DeleteTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var response = new DeleteCommandResponse<TransactionBaseDTO>();

        var transaction = await _unitOfWork.TransactionRepository.GetTransactionWithWalletAsync(request.Id);

        if (transaction is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Id}]",
                PropertyName = nameof(request.Id)
            }, nameof(Domain.Entities.Transaction));

        if (transaction.IsBankTransaction)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Can not delete bank transaction",
                PropertyName = nameof(transaction.IsBankTransaction)
            });

        var deletedObj = _mapper.Map<TransactionBaseDTO>(transaction);

        
        IncreaseWalletBalance(transaction.Wallet, transaction.Amount);
        await IncreaseBudgetBalance(transaction.CategoryId, transaction.Amount, transaction.UserId);

        _unitOfWork.TransactionRepository.Delete(transaction);

        response.Success = true;
        response.Message = "Deleted successfully";
        response.DeletedObj = deletedObj;
        response.Id = deletedObj.Id;
        await _unitOfWork.SaveAsync();

        return response;
    }

    private void IncreaseWalletBalance(Domain.Entities.Wallet wallet, decimal amount)
    {
        wallet.Balance += amount;
    }

    private async Task IncreaseBudgetBalance(Guid categoryId, decimal amount, Guid userId)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(categoryId);

        foreach (var budget in budgets)
        {
            if (budget.IsPublic || budget.UserId == userId)
                budget.Balance += amount;
        }
    }
}