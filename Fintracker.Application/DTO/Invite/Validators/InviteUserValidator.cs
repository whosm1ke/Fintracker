using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.User.Requests.Commands;
using FluentValidation;

namespace Fintracker.Application.DTO.Invite.Validators;

public class InviteUserValidator : AbstractValidator<InviteUserCommand>
{
    public InviteUserValidator(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.WalletId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"Wallet with id does not exist [{x.WalletId}]")
            .MustAsync(async (dto, id, _) => !await userRepository.HasMemberWallet(id, dto.UserEmail))
            .WithMessage("User already has access to that wallet");

        RuleFor(x => x.UserEmail)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .NotEqual(x => x.WhoInvited)
            .WithMessage("Can not invite yourself")
            .Matches(
                @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")
            .WithMessage("Has wrong format");
    }
}