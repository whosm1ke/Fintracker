using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class CreateWalletDtoValidator : AbstractValidator<CreateWalletDTO>
{
    public CreateWalletDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        Include(new WalletBaseDtoValidator(unitOfWork));

        RuleFor(x => x.OwnerId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.OwnerId}]");
    }
}