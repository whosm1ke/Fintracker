using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Enums;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class
    DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand,
    DeleteCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrencyConverter _currencyConverter;

    public DeleteTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrencyConverter currencyConverter)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currencyConverter = currencyConverter;
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

        var transCurrency = await _unitOfWork.CurrencyRepository.GetAsync(transaction.CurrencyId);

        var deletedObj = _mapper.Map<TransactionBaseDTO>(transaction);
        transaction.Category.TransactionCount -= 1;

        await UpdateWallet(transaction.Wallet, transaction.Amount, transCurrency!.Symbol, transaction.Category.Type);
        await IncreaseBudgetBalance(transaction.WalletId, transaction.UserId, transaction);


        _unitOfWork.TransactionRepository.Delete(transaction);
        await _unitOfWork.SaveAsync();


        response.Success = true;
        response.Message = "Deleted successfully";
        response.DeletedObj = deletedObj;
        response.Id = deletedObj.Id;

        return response;
    }

    private async Task UpdateWallet(Domain.Entities.Wallet wallet, decimal amount,
        string transactionCurrencySymbol, CategoryType transType)
    {
        ConvertCurrencyDTO? convertedCurrency = null;
        if (wallet.Currency.Symbol != transactionCurrencySymbol)
            convertedCurrency =
                await _currencyConverter.Convert(transactionCurrencySymbol, wallet.Currency.Symbol, amount);

        if (transType == CategoryType.INCOME)
        {
            wallet.Balance -= convertedCurrency?.Value ?? amount;
        }
        else
        {
            wallet.Balance += convertedCurrency?.Value ?? amount;
            wallet.TotalSpent -= convertedCurrency?.Value ?? amount;
        }
    }

    private async Task IncreaseBudgetBalance(Guid walletId, Guid userId, Domain.Entities.Transaction transToDelete)
    {
        var budgetsByWalletId = await _unitOfWork.BudgetRepository.GetByWalletIdAsync(walletId, userId, null);
        foreach (var budget in budgetsByWalletId)
        {
            if (budget.Categories.All(c => c.Id != transToDelete.CategoryId)) continue;

            var transactionCurrencySymbol = transToDelete.Currency.Symbol;
            var transactionAmount = transToDelete.Amount;

            ConvertCurrencyDTO? convertedResult = null;
            if (transToDelete.CurrencyId != budget.CurrencyId)
                convertedResult = await _currencyConverter.Convert(transactionCurrencySymbol, budget.Currency.Symbol, transactionAmount);

            decimal totalSpent = convertedResult?.Value ?? transactionAmount;

            budget.Transactions.Remove(transToDelete);
            budget.TotalSpent -= totalSpent;
            budget.Balance += totalSpent;
        }
    }
}