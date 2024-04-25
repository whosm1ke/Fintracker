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


        await UpdateWallet(transaction.Wallet, transaction.Amount, transCurrency!.Symbol, transaction.Category.Type);
        await IncreaseBudgetBalance(transaction.WalletId);


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

    private async Task IncreaseBudgetBalance(Guid walletId)
    {
        var budgetsByWalletId = await _unitOfWork.BudgetRepository.GetByWalletIdAsync(walletId, null);
        foreach (var budget in budgetsByWalletId)
        {
            var transactions =
                await _unitOfWork.TransactionRepository.GetByWalletIdInRangeAsync(walletId, budget.StartDate,
                    budget.EndDate);
            
            if (transactions.Count == 0) return;
            var filteredTransactions = transactions.Where(x => budget.Categories.Any(c => c.Id == x.CategoryId))
                .ToList();
            
            if (filteredTransactions.Count == 0) return;
            var transactionCurrencySymbols = filteredTransactions.Select(x => x.Currency.Symbol);
            var transactionAmounts = filteredTransactions.Select(x => x.Amount);

            var convertedResult =
                await _currencyConverter.Convert(transactionCurrencySymbols, budget.Currency.Symbol,
                    transactionAmounts);

            decimal totalSpent = 0;
            convertedResult.ForEach(x => totalSpent += x.Value);
            filteredTransactions.ForEach(x => budget.Transactions.Remove(x));

            budget.TotalSpent -= totalSpent;
            budget.Balance += totalSpent;

            await _unitOfWork.SaveAsync();
        }
    }
}