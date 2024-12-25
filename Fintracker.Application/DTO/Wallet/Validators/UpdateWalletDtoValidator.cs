using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class UpdateWalletDtoValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        RuleFor(x => x.Wallet)
            .SetValidator(new WalletBaseDtoValidator(unitOfWork))
            .OverridePropertyName(string.Empty);

        RuleFor(x => x.Wallet.Id)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateWalletCommand.Wallet.Id))
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not exist [{x.Wallet.Id}]");

        RuleFor(x => x.Wallet.UserIds)
            .MustAsync(async (userIds, _) =>
            {
                bool isExisting = false;
                foreach (var userId in userIds)
                {
                    isExisting = await userRepository.ExistsAsync(userId);
                }

                return isExisting;
            })
            .WithMessage("One of provided users does not exists")
            .When(x => x.Wallet.UserIds.Any());
    }
}