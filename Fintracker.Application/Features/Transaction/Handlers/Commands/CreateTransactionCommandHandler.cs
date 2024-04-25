﻿using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
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


        await DecreaseBalanceInWallet(transaction.WalletId, transaction.Amount, transCurrency!.Symbol);
        await DecreaseBalanceInBudgets(transaction.CategoryId, transaction.Amount, transaction.UserId,
            transaction.WalletId, transCurrency.Symbol);


        response.Success = true;
        response.Message = "Created successfully";
        response.Id = transaction.Id;
        response.CreatedObject = request.Transaction;

        await _unitOfWork.SaveAsync();


        return response;
    }

    private async Task DecreaseBalanceInBudgets(Guid categoryId, decimal amount, Guid userId, Guid walletId,
        string transactionCurrencySymbol)
    {
        var budgets = await _unitOfWork.BudgetRepository.GetBudgetsByCategoryId(categoryId);
        var currencySymbols = budgets.Select(x => x.Currency.Symbol);

        var convertedCurrencies =
            await _currencyConverter.Convert(transactionCurrencySymbol, currencySymbols, amount);

        foreach (var budget in budgets)
        {
            if ((budget.IsPublic || budget.UserId == userId) && budget.WalletId == walletId)
            {
                var convertedCurrency = convertedCurrencies.Find(x => x?.To == budget.Currency.Symbol);
                budget.Balance -= convertedCurrency?.Value ?? amount;
                budget.TotalSpent += convertedCurrency?.Value ?? amount;
            }
        }
    }

    private async Task DecreaseBalanceInWallet(Guid walletId, decimal amount, string transactionCurrencySymbol)
    {
        var wallet = await _unitOfWork.WalletRepository.GetWalletWithCurrency(walletId);


        ConvertCurrencyDTO? convertedCurrency = null;
        if (wallet!.Currency.Symbol != transactionCurrencySymbol)
            convertedCurrency = await _currencyConverter.Convert(transactionCurrencySymbol, wallet.Currency.Symbol, amount);

        wallet.Balance -= convertedCurrency?.Value ?? amount;
        wallet.TotalSpent += convertedCurrency?.Value ?? amount;
    }
}