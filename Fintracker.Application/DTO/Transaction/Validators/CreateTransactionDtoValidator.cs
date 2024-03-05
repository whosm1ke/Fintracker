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
            .WithMessage($"{nameof(CreateTransactionDTO.UserId)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(CreateTransactionDTO.UserId)} can not be blank")
            .MustAsync((id, _) => userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id [{x.UserId}] does not exists");

        RuleFor(x => x.WalletId)
            .NotNull()
            .WithMessage($"{nameof(CreateTransactionDTO.WalletId)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(CreateTransactionDTO.WalletId)} can not be blank")
            .MustAsync((id, _) => unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id [{x.WalletId}] does not exists");
    }
}