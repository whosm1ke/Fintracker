using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.User.Requests.Commands;
using FluentValidation;

namespace Fintracker.Application.DTO.Invite.Validators;

public class AddUserToWalletValidator : AbstractValidator<AddUserToWalletCommand>
{
    public AddUserToWalletValidator(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.WalletId)
            .NotNull()
            .WithMessage($"{nameof(AddUserToWalletCommand.WalletId)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(AddUserToWalletCommand.WalletId)} can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"Wallet with id [{x.WalletId}] does not exist")
            .MustAsync(async (id, _) => !await userRepository.HasMemberWallet(id))
            .WithMessage(x => $"User already has access to that wallet");

        RuleFor(x => x.Token)
            .NotNull()
            .WithMessage($"{nameof(AddUserToWalletCommand.Token)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(AddUserToWalletCommand.Token)} can not be blank");
    }
}