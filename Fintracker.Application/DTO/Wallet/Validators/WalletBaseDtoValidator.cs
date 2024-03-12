using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class WalletBaseDtoValidator : AbstractValidator<IWalletDto>
{
    public WalletBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Balance)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .LessThanOrEqualTo(WalletConstraints.MaxBalance)
            .WithMessage($"Should be less than or equal to {WalletConstraints.MaxBalance}");

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MinimumLength(WalletConstraints.MinNameLength)
            .WithMessage($"Should be greater then {WalletConstraints.MinNameLength}")
            .MaximumLength(WalletConstraints.MaxNameLength)
            .WithMessage($"Should be less then {WalletConstraints.MaxNameLength}");

        RuleFor(x => x.CurrencyId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.CurrencyRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id does not exist [{x.CurrencyId}]");
    }
}