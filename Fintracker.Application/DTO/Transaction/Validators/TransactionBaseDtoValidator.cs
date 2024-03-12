using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Transaction.Validators;

public class TransactionBaseDtoValidator : AbstractValidator<ITransactionDto>
{
    public TransactionBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .GreaterThan(TransactionConstraints.MinimalTransactionAmount)
            .WithMessage($"Should be greater than {TransactionConstraints.MinimalTransactionAmount}");

        RuleFor(x => x.Label)
            .Length(0, TransactionConstraints.MaximumLabelLength)
            .WithMessage($"Is optional, but should be less than {TransactionConstraints.MaximumLabelLength}");

        RuleFor(x => x.Note)
            .Length(0, TransactionConstraints.MaximumNoteLength)
            .WithMessage($"Is optional, but should be less than {TransactionConstraints.MaximumNoteLength}");

        RuleFor(x => x.CategoryId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.CategoryRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Category)} with id does not exist [{x.CategoryId}]");

        RuleFor(x => x.CurrencyId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await unitOfWork.CurrencyRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id does not exist [{x.CurrencyId}]");
    }
}