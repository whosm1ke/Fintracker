using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Fintracker.Application.DTO.Invite.Validators;

public class AddUserToWalletValidator : AbstractValidator<AddUserToWalletCommand>
{
    public AddUserToWalletValidator(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.WalletId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"Wallet with id does not exist [{x.WalletId}]");

        RuleFor(x => x.UserId)
            .ApplyCommonRules(x => x.UserId != Guid.Empty)
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"User with id does not exist [{x.UserId}]");;
    }
}