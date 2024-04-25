using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
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


        await IncreaseWalletBalance(transaction.Wallet, transaction.Amount, transCurrency!.Symbol);
        await IncreaseBudgetBalance(transaction.WalletId);


        _unitOfWork.TransactionRepository.Delete(transaction);
        await _unitOfWork.SaveAsync();


        response.Success = true;
        response.Message = "Deleted successfully";
        response.DeletedObj = deletedObj;
        response.Id = deletedObj.Id;

        return response;
    }

    private async Task IncreaseWalletBalance(Domain.Entities.Wallet wallet, decimal amount,
        string transactionCurrencySymbol)
    {
        ConvertCurrencyDTO? convertedCurrency = null;
        if (wallet.Currency.Symbol != transactionCurrencySymbol)
            convertedCurrency =
                await _currencyConverter.Convert(transactionCurrencySymbol, wallet.Currency.Symbol, amount);

        wallet.Balance += convertedCurrency?.Value ?? amount;
        wallet.TotalSpent -= convertedCurrency?.Value ?? amount;
    }

    private async Task IncreaseBudgetBalance(Guid walletId)
    {
        var budgetsByWalletId = await _unitOfWork.BudgetRepository.GetByWalletIdAsync(walletId, null);
        foreach (var budget in budgetsByWalletId)
        {
            var transactions =
                await _unitOfWork.TransactionRepository.GetByWalletIdInRangeAsync(walletId, budget.StartDate,
                    budget.EndDate);

            var filteredTransactions = transactions.Where(x => budget.Categories.Any(c => c.Id == x.CategoryId))
                .ToList();
            var transactionCurrencySymbols = filteredTransactions.Select(x => x.Currency.Symbol);
            var transactionAmounts = filteredTransactions.Select(x => x.Amount);

            var convertedResult =
                await _currencyConverter.Convert(transactionCurrencySymbols, budget.Currency.Symbol,
                    transactionAmounts);

            decimal totalSpent = 0;
            convertedResult.ForEach(x => totalSpent += x.Value);

            budget.TotalSpent -= totalSpent;
            budget.Balance += totalSpent;

            await _unitOfWork.SaveAsync();
        }
    }
}