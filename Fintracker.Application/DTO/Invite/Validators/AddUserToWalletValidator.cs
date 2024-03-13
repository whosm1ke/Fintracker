using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Invite.Validators;

public class AddUserToWalletValidator : AbstractValidator<AddUserToWalletCommand>
{
    public AddUserToWalletValidator(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.WalletId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"Wallet with id does not exist [{x.WalletId}]");

        RuleFor(x => x.Token)
            .ApplyCommonRules();
    }
}