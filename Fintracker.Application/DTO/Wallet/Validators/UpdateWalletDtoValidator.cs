using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class UpdateWalletDtoValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Wallet)
            .SetValidator(new WalletBaseDtoValidator(unitOfWork))
            .OverridePropertyName(string.Empty);

        RuleFor(x => x.Wallet.Id)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateWalletCommand.Wallet.Id))
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not exist [{x.Wallet.Id}]");
    }
}