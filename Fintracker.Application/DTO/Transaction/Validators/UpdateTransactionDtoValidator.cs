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
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.TransactionRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Transaction)} with id does not exists [{x.Id}]");
    }
}