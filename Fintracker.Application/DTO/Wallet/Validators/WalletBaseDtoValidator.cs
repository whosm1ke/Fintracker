using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class WalletBaseDtoValidator : AbstractValidator<IWalletDto>
{
    public WalletBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Balance)
            .NotNull()
            .WithMessage($"{nameof(IWalletDto.Balance)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(IWalletDto.Balance)} can not be blank")
            .LessThanOrEqualTo(WalletConstraints.MaxBalance)
            .WithMessage(
                $"{nameof(IWalletDto.Balance)} should be less than or equal to {WalletConstraints.MaxBalance}");

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage($"{nameof(IWalletDto.Name)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(IWalletDto.Name)} can not be blank")
            .MinimumLength(WalletConstraints.MinNameLength)
            .WithMessage(
                $"Length of {nameof(IWalletDto.Name)} should be greater then {WalletConstraints.MinNameLength}")
            .MaximumLength(WalletConstraints.MaxNameLength)
            .WithMessage($"Length of {nameof(IWalletDto.Name)} should be less then {WalletConstraints.MaxNameLength}");

        RuleFor(x => x.CurrencyId)
            .NotNull()
            .WithMessage($"{nameof(IWalletDto.CurrencyId)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(IWalletDto.CurrencyId)} can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.CurrencyRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id [{x.CurrencyId}] does not exists");
    }
}