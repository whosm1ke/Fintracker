using AutoMapper;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Currency;
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

        if (wallet.IsBanking && wallet.Balance.CompareTo(request.Wallet.StartBalance) != 0)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Can not change balance in banking wallet",
                PropertyName = nameof(wallet.Balance)
            });

        await UpdateWallet(wallet, request.Wallet);

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

    
    
    private async Task UpdateWallet(Domain.Entities.Wallet oldWallet, UpdateWalletDTO newWallet)
    {
        oldWallet.StartBalance = newWallet.StartBalance;
        oldWallet.Balance = newWallet.StartBalance;
        oldWallet.TotalSpent = 0;

        foreach (var userId in newWallet.UserIds)
        {
            var userToDelete = oldWallet.Users.FirstOrDefault(u => u.Id == userId);
            if(userToDelete is not null)
                oldWallet.Users.Remove(userToDelete);

            if (newWallet.DeleteUserTransaction)
            {
                var transactionsToDelete = oldWallet.Transactions.Where(t => t.UserId == userId).ToList();
                
                transactionsToDelete.ForEach(t => oldWallet.Transactions.Remove(t));
            }
        }


        var transactionCurrencySymbols = oldWallet.Transactions.Select(x => x.Currency.Symbol).ToList();
        var transactionAmounts = oldWallet.Transactions.Select(x =>
        {
            if (x.Category.Type == CategoryType.INCOME)
                return x.Amount;
            return x.Amount * -1;
        }).ToList();

        //Calculating new balance and total spent
        decimal totalSpent = 0;
        if (oldWallet.CurrencyId != newWallet.CurrencyId)
        {
            var newCurrency = await _unitOfWork.CurrencyRepository.GetAsync(newWallet.CurrencyId);
            oldWallet.Currency = newCurrency!;
            List<ConvertCurrencyDTO?> convertedResult =
                await _currencyConverter.Convert(transactionCurrencySymbols, newCurrency!.Symbol, transactionAmounts);

            convertedResult.ForEach(x => totalSpent += x.Value);
        }
        else
        {
            foreach (var transaction in oldWallet.Transactions)
            {
                if (transaction.Category.Type == CategoryType.INCOME)
                    totalSpent += transaction.Amount;
                else
                    totalSpent += transaction.Amount * -1;
            }
        }

        oldWallet.Balance = newWallet.StartBalance + totalSpent;
        oldWallet.TotalSpent = totalSpent > 0 ? 0 : Math.Abs(totalSpent);
    }
}