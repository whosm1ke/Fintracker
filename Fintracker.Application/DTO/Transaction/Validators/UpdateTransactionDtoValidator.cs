using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Transaction.Validators;

public class UpdateTransactionDtoValidator : AbstractValidator<UpdateTransactionDTO>
{
    public UpdateTransactionDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new TransactionBaseDtoValidator(unitOfWork));

        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage($"{nameof(UpdateTransactionDTO.Id)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(UpdateTransactionDTO.Id)} can not be blank")
            .MustAsync((id, _) => unitOfWork.TransactionRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Transaction)} with id [{x.Id}] does not exists");
    }
}