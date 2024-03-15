using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Fintracker.Application.DTO.Invite.Validators;

public class InviteUserValidator : AbstractValidator<InviteUserCommand>
{
    public InviteUserValidator(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.WalletId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"Wallet with id does not exist [{x.WalletId}]")
            .MustAsync(async (dto, id, _) => !await userRepository.HasMemberWallet(id, dto.UserEmail))
            .WithMessage("User already has access to that wallet");

        RuleFor(x => x.UserEmail)
            .ApplyCommonRules(x => x.UserEmail is not null)
            .NotEqual(x => x.WhoInvited)
            .WithMessage("Can not invite yourself")
            .ApplyEmail();
    }
}