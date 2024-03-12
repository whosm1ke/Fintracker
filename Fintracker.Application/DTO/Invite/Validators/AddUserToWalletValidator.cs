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
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"Wallet with id does not exist [{x.WalletId}]");

        RuleFor(x => x.Token)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank");
    }
}