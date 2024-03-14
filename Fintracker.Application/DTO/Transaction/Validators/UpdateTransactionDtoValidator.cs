using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Transaction.Validators;

public class UpdateTransactionDtoValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Transaction)
            .SetValidator(new TransactionBaseDtoValidator(unitOfWork))
            .OverridePropertyName(string.Empty);

        RuleFor(x => x.Transaction.Id)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateTransactionCommand.Transaction.Id))
            .MustAsync(async (id, _) => await unitOfWork.TransactionRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Transaction)} with id does not exists [{x.Transaction.Id}]");
    }
}