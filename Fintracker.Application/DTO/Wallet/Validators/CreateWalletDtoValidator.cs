using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class CreateWalletDtoValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        RuleFor(x => x.Wallet)
            .SetValidator(new WalletBaseDtoValidator(unitOfWork))
            .OverridePropertyName(string.Empty);

        RuleFor(x => x.Wallet.OwnerId)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateWalletCommand.Wallet.OwnerId))
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.Wallet.OwnerId}]");

    }
}