using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Transaction.Validators;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        RuleFor(x => x.Transaction)
            .SetValidator(new TransactionBaseDtoValidator(unitOfWork))
            .OverridePropertyName(string.Empty);


        RuleFor(x => x.Transaction.UserId)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateTransactionCommand.Transaction.UserId))
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.Transaction.UserId}]");

        RuleFor(x => x.Transaction.WalletId)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateTransactionCommand.Transaction.WalletId))
            .MustAsync(async (id, _) => await unitOfWork.WalletRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not exist [{x.Transaction.WalletId}]");

        RuleFor(x => x.Transaction.IsBankTransaction)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateTransactionCommand.Transaction.IsBankTransaction));
    }
}