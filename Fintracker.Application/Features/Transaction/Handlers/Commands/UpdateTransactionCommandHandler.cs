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
    UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand,
    UpdateCommandResponse<TransactionBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrencyConverter _currencyConverter;

    public UpdateTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrencyConverter currencyConverter)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currencyConverter = currencyConverter;
    }

    public async Task<UpdateCommandResponse<TransactionBaseDTO>> Handle(UpdateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<TransactionBaseDTO>();

        var transaction = await _unitOfWork.TransactionRepository.GetTransactionAsync(request.Transaction.Id);

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

        var newCurrency = await _unitOfWork.CurrencyRepository.GetAsync(request.Transaction.CurrencyId);
        var newCategory = await _unitOfWork.CategoryRepository.GetAsync(request.Transaction.CategoryId);
        await UpdateWalletBalance(transaction.Wallet, request.Transaction.Amount, transaction.Amount,
            transaction.Currency.Symbol, newCurrency!.Symbol, transaction.Category.Type, newCategory!.Type);

        var oldObject = _mapper.Map<TransactionBaseDTO>(transaction);
        _unitOfWork.TransactionRepository.Update(transaction);
        _mapper.Map(request.Transaction, transaction);


        await _unitOfWork.SaveAsync();
        await UpdateBudgets(transaction.WalletId, transaction.UserId);

        var newObject = _mapper.Map<TransactionBaseDTO>(transaction);
        response.Success = true;
        response.Message = "Updated successfully";
        response.Old = oldObject;
        response.New = newObject;
        response.Id = request.Transaction.Id;


        return response;
    }


    private async Task UpdateWalletBalance(Domain.Entities.Wallet wallet, decimal newAmount, decimal oldAmount,
        string oldCurrencySymbol, string newCurrencySymbol, CategoryType oldTransType, CategoryType newTransType)
    {
        ConvertCurrencyDTO? convertedCurrencyNewAmount = null;
        ConvertCurrencyDTO? convertedCurrencyOldAmount = null;

        if (newCurrencySymbol != oldCurrencySymbol)
        {
            convertedCurrencyNewAmount =
                await _currencyConverter.Convert(newCurrencySymbol, wallet.Currency.Symbol, newAmount);
            convertedCurrencyOldAmount =
                await _currencyConverter.Convert(oldCurrencySymbol, wallet.Currency.Symbol, oldAmount);
        }

        if (oldTransType == CategoryType.EXPENSE && newTransType == CategoryType.EXPENSE)
        {
            wallet.Balance += convertedCurrencyOldAmount?.Value ?? oldAmount;
            wallet.Balance -= convertedCurrencyNewAmount?.Value ?? newAmount;
            wallet.TotalSpent -= convertedCurrencyOldAmount?.Value ?? oldAmount;
            wallet.TotalSpent += convertedCurrencyNewAmount?.Value ?? newAmount;
        }

        if (oldTransType == CategoryType.EXPENSE && newTransType == CategoryType.INCOME)
        {
            wallet.Balance += convertedCurrencyOldAmount?.Value ?? oldAmount;
            wallet.Balance += convertedCurrencyNewAmount?.Value ?? newAmount;
            wallet.TotalSpent -= convertedCurrencyOldAmount?.Value ?? oldAmount;
        }

        if (oldTransType == CategoryType.INCOME && newTransType == CategoryType.INCOME)
        {
            wallet.Balance -= convertedCurrencyOldAmount?.Value ?? oldAmount;
            wallet.Balance += convertedCurrencyNewAmount?.Value ?? newAmount;
        }

        if (oldTransType == CategoryType.INCOME && newTransType == CategoryType.EXPENSE)
        {
            wallet.Balance -= convertedCurrencyOldAmount?.Value ?? oldAmount;
            wallet.Balance -= convertedCurrencyNewAmount?.Value ?? newAmount;
            wallet.TotalSpent += convertedCurrencyNewAmount?.Value ?? newAmount;
        }
    }

    private async Task UpdateBudgets(Guid walletId, Guid userId)
    {
        var budgetsByWalletId = await _unitOfWork.BudgetRepository.GetByWalletIdAsync(walletId, userId, null);
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

            budget.TotalSpent = totalSpent;
            budget.Balance = budget.StartBalance - totalSpent;

            await _unitOfWork.SaveAsync();
        }
    }
}