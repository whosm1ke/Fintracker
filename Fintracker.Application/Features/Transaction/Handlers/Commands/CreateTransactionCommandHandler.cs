using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Enums;
using MediatR;

namespace Fintracker.Application.Features.Transaction.Handlers.Commands;

public class
    CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand,
    CreateCommandResponse<CreateTransactionDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrencyConverter _currencyConverter;

    public CreateTransactionCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrencyConverter currencyConverter)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currencyConverter = currencyConverter;
    }

    public async Task<CreateCommandResponse<CreateTransactionDTO>> Handle(CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var transCurrency = await _unitOfWork.CurrencyRepository.GetAsync(request.Transaction.CurrencyId);

        var response = new CreateCommandResponse<CreateTransactionDTO>();
        var transaction = _mapper.Map<Domain.Entities.Transaction>(request.Transaction);

        await _unitOfWork.TransactionRepository.AddAsync(transaction);

        var transCategory = await _unitOfWork.CategoryRepository.GetAsync(request.Transaction.CategoryId);
        ++transCategory!.TransactionCount;

        await UpdateWallet(transaction, transaction.WalletId, transaction.Amount, transCurrency!.Symbol,
            transCategory.Type);
        await DecreaseBalanceInBudgets(transaction.CategoryId, transaction.Amount, transaction.UserId,
            transaction.WalletId, transCurrency.Symbol, transaction, request.Transaction.Date);


        response.Success = true;
        response.Message = "Created successfully";
        response.Id = transaction.Id;
        response.CreatedObject = request.Transaction;

        await _unitOfWork.SaveAsync();


        return response;
    }

    private async Task DecreaseBalanceInBudgets(Guid categoryId, decimal amount, Guid userId, Guid walletId,
        string transactionCurrencySymbol, Domain.Entities.Transaction transaction, DateTime transDate)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(categoryId, userId);
        var currencySymbols = budgets.Select(x => x.Currency.Symbol);

        var convertedCurrencies =
            await _currencyConverter.Convert(transactionCurrencySymbol, currencySymbols, amount);

        foreach (var budget in budgets)
        {
            if ((budget.IsPublic || budget.OwnerId == userId) && budget.WalletId == walletId &&
                transDate >= budget.StartDate && transDate <= budget.EndDate)
            {
                var convertedCurrency = convertedCurrencies.Find(x => x?.To == budget.Currency.Symbol);
                budget.Balance -= convertedCurrency?.Value ?? amount;
                budget.TotalSpent += convertedCurrency?.Value ?? amount;
                budget.Transactions.Add(transaction);
            }
        }
    }

    private async Task UpdateWallet(Domain.Entities.Transaction transaction, Guid walletId, decimal amount,
        string transactionCurrencySymbol,
        CategoryType transType)
    {
        var wallet = await _unitOfWork.WalletRepository.GetWalletById(walletId);
        ConvertCurrencyDTO? convertedCurrency = null;
        if (wallet!.Currency.Symbol != transactionCurrencySymbol)
        {
            convertedCurrency =
                await _currencyConverter.Convert(transactionCurrencySymbol, wallet.Currency.Symbol, amount);
        }

        if (transType == CategoryType.EXPENSE)
        {
            wallet.Balance -= convertedCurrency?.Value ?? amount;
            wallet.TotalSpent += convertedCurrency?.Value ?? amount;
        }
        else
        {
            wallet.Balance += convertedCurrency?.Value ?? amount;
        }
        
        transaction.AmountInWalletCurrency = convertedCurrency?.Value ?? amount;

    }
}