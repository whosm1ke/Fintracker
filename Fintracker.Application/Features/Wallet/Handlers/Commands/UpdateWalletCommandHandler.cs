using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Enums;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Commands;

public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, UpdateCommandResponse<WalletBaseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrencyConverter _currencyConverter;

    public UpdateWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrencyConverter currencyConverter)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currencyConverter = currencyConverter;
    }

    public async Task<UpdateCommandResponse<WalletBaseDTO>> Handle(UpdateWalletCommand request,
        CancellationToken cancellationToken)
    {
        var response = new UpdateCommandResponse<WalletBaseDTO>();

        var wallet = await _unitOfWork.WalletRepository.GetWalletById(request.Wallet.Id);

        if (wallet is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"Was not found by id [{request.Wallet.Id}]",
                PropertyName = nameof(request.Wallet.Id)
            }, nameof(Domain.Entities.Wallet));

        if (wallet.IsBanking && wallet.StartBalance.CompareTo(request.Wallet.StartBalance) != 0)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Can not change balance in banking wallet",
                PropertyName = nameof(wallet.StartBalance)
            });

        await UpdateWalletBalance(wallet, request.Wallet);
        await UpdateBudgetsBalance(wallet, request.Wallet);

        var oldObject = _mapper.Map<WalletBaseDTO>(wallet);
        _mapper.Map(request.Wallet, wallet);
        _unitOfWork.WalletRepository.Update(wallet);

        await _unitOfWork.SaveAsync();

        var newObject = _mapper.Map<WalletBaseDTO>(wallet);

        response.Success = true;
        response.Message = "Updated successfully";
        response.Old = oldObject;
        response.New = newObject;
        response.Id = request.Wallet.Id;


        return response;
    }

    private async Task UpdateBudgetsBalance(Domain.Entities.Wallet oldWallet, UpdateWalletDTO newWallet)
    {
        foreach (var userId in newWallet.UserIds)
        {
            var user = oldWallet.Users.FirstOrDefault(u => u.Id == userId);
            if (user is null) continue;

            foreach (var memberBudget in user.MemberBudgets)
            {
                memberBudget.TotalSpent = 0;
                var userTransactions = memberBudget.Transactions.Where(t => t.UserId == userId).ToList();
                var transactionDetails =
                    GetTransactionDetails(userTransactions);


                var convertedResult = await _currencyConverter.Convert(transactionDetails.Symbols,
                    memberBudget.Currency.Symbol,
                    transactionDetails.Amounts);


                var totalSpent = convertedResult.Sum(x => x.Value);

                memberBudget.Balance -= totalSpent;
                memberBudget.TotalSpent = memberBudget.StartBalance - memberBudget.Balance;

            }
            
            RemoveUserFromCollections(user, oldWallet);
        }
    }
    
    private void RemoveUserFromCollections(Domain.Entities.User user, Domain.Entities.Wallet wallet)
    {
        foreach (var memberBudget in user.MemberBudgets)
        {
            memberBudget.Members.Remove(user);
        }

        var useTransactions = wallet.Transactions.Where(t => t.UserId == user.Id);
        foreach (var transaction in useTransactions)
        {
            wallet.Transactions.Remove(transaction);
        }
        wallet.Users.Remove(user);
    }



    private async Task UpdateWalletBalance(Domain.Entities.Wallet oldWallet, UpdateWalletDTO newWallet)
    {
        ResetWallet(oldWallet, newWallet.StartBalance);

        var transactionDetails = GetTransactionDetails(oldWallet.Transactions);

        decimal totalSpent = await CalculateTotalSpent(oldWallet, newWallet, transactionDetails);

        UpdateWalletAfterCalculations(oldWallet, newWallet, totalSpent);
    }

    private void ResetWallet(Domain.Entities.Wallet wallet, decimal startBalance)
    {
        wallet.StartBalance = startBalance;
        wallet.Balance = startBalance;
        wallet.TotalSpent = 0;
    }

    private (List<string> Symbols, List<decimal> Amounts) GetTransactionDetails(
        ICollection<Domain.Entities.Transaction> transactions)
    {
        var symbols = transactions.Select(x => x.Currency.Symbol).ToList();
        var amounts = transactions.Select(x => x.Category.Type == CategoryType.INCOME ? x.Amount : -x.Amount).ToList();

        return (symbols, amounts);
    }

    private async Task<decimal> CalculateTotalSpent(Domain.Entities.Wallet oldWallet, UpdateWalletDTO newWallet,
        (List<string> Symbols, List<decimal> Amounts) transactionDetails)
    {
        decimal totalSpent = 0;

        if (oldWallet.CurrencyId != newWallet.CurrencyId)
        {
            var newCurrency = await _unitOfWork.CurrencyRepository.GetAsync(newWallet.CurrencyId);
            oldWallet.Currency = newCurrency!;

            var convertedResult = await _currencyConverter.Convert(transactionDetails.Symbols, newCurrency!.Symbol,
                transactionDetails.Amounts);
            totalSpent = convertedResult.Sum(x => x.Value);
        }
        else
        {
            totalSpent = transactionDetails.Amounts.Sum();
        }

        return totalSpent;
    }

    private void UpdateWalletAfterCalculations(Domain.Entities.Wallet oldWallet, UpdateWalletDTO newWallet,
        decimal totalSpent)
    {
        oldWallet.Balance = newWallet.StartBalance + totalSpent;
        oldWallet.TotalSpent = totalSpent > 0 ? 0 : Math.Abs(totalSpent);
    }
}