using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Wallet.Validators;

public class UpdateWalletDtoValidator : AbstractValidator<UpdateWalletDTO>
{
    public UpdateWalletDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new WalletBaseDtoValidator(unitOfWork));

        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not exist [{x.Id}]");
    }
}