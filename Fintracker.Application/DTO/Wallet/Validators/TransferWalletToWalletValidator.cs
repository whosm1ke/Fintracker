using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class TransferWalletToWalletValidator : AbstractValidator<TransferMoneyFromWalletToWalletCommand>, INotGetRequest
{
    public TransferWalletToWalletValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.FromWalletId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not " +
                              $"exist [{x.FromWalletId}]");
        
        RuleFor(x => x.ToWalletId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not " +
                              $"exist [{x.ToWalletId}]");
        
        RuleFor(x => x.Amount)
            .ApplyCommonRules()
            .ApplyGreaterLessThan(TransactionConstraints.MinimalTransactionAmount, TransactionConstraints.MaximumTransactionAmount);
    }
}