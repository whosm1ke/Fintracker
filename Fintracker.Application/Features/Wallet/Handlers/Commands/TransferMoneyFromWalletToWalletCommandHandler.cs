using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.Wallet.Handlers.Commands;

public class
    TransferMoneyFromWalletToWalletCommandHandler : IRequestHandler<TransferMoneyFromWalletToWalletCommand,
    BaseCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransferMoneyFromWalletToWalletCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseCommandResponse> Handle(TransferMoneyFromWalletToWalletCommand request,
        CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();

        var fromWallet = await _unitOfWork.WalletRepository.GetWalletById(request.FromWalletId);

        if (fromWallet!.IsBanking)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Can not transfer money from banking wallet",
                PropertyName = nameof(Domain.Entities.Wallet)
            });

        var toWallet = await _unitOfWork.WalletRepository.GetWalletById(request.ToWalletId);
        
        if (toWallet!.IsBanking)
            throw new BadRequestException(new ExceptionDetails
            {
                ErrorMessage = "Can not transfer money to banking wallet",
                PropertyName = nameof(Domain.Entities.Wallet)
            });
        var incomeCategoryId = await _unitOfWork.CategoryRepository.GetDefaultBankIncomeCategoryId(fromWallet.OwnerId);
        var expenseCategoryId = await _unitOfWork.CategoryRepository.GetDefaultBankExpenseCategoryId(fromWallet.OwnerId);

        fromWallet!.Balance -= request.Amount;
        toWallet!.Balance += request.Amount;

        var fromTransaction = new Domain.Entities.Transaction();
        fromTransaction.UserId = fromWallet.OwnerId;
        fromTransaction.Amount = request.Amount;
        fromTransaction.WalletId = fromWallet.Id;
        fromTransaction.CurrencyId = fromWallet.CurrencyId;
        fromTransaction.CategoryId = expenseCategoryId;
        fromTransaction.Note = $"Transfer to wallet {toWallet.Name}";
        fromTransaction.Label = $"Transfer to wallet {toWallet.Name}";

        fromWallet.Transactions.Add(fromTransaction);

        var toTransaction = new Domain.Entities.Transaction();
        toTransaction.UserId = toWallet.OwnerId;
        toTransaction.Amount = request.Amount;
        toTransaction.WalletId = toWallet.Id;
        toTransaction.CurrencyId = fromWallet.CurrencyId;
        toTransaction.CategoryId = incomeCategoryId;
        toTransaction.Note = $"Income from wallet {fromWallet.Name}";
        toTransaction.Label = $"Income from wallet {fromWallet.Name}";

        toWallet.Transactions.Add(toTransaction);

        _unitOfWork.WalletRepository.Update(fromWallet);
        _unitOfWork.WalletRepository.Update(toWallet);

        await _unitOfWork.SaveAsync();

        response.Id = fromWallet.Id;
        response.Message = "Transfer sent successfully";
        response.Success = true;

        return response;
    }
}