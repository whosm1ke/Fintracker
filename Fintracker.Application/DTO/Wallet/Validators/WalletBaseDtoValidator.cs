using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class WalletBaseDtoValidator : AbstractValidator<IWalletDto>
{
    public WalletBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Balance)
            .ApplyCommonRules()
            .ApplyLessThanEqual(WalletConstraints.MaxBalance);

        RuleFor(x => x.Name)
            .ApplyCommonRules()
            .ApplyMinMaxLength(WalletConstraints.MinNameLength, WalletConstraints.MaxNameLength);

        RuleFor(x => x.CurrencyId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.CurrencyRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id does not exist [{x.CurrencyId}]");
    }
}