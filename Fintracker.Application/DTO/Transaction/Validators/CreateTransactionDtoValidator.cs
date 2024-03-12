using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Transaction.Validators;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDTO>
{
    public CreateTransactionDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        Include(new TransactionBaseDtoValidator(unitOfWork));

        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.UserId}]");

        RuleFor(x => x.WalletId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not exist [{x.WalletId}]");
    }
}