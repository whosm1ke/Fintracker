using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Transaction.Validators;

public class TransactionBaseDtoValidator : AbstractValidator<ITransactionDto>
{
    public TransactionBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Amount)
            .ApplyCommonRules()
            .ApplyGreaterLessThan(TransactionConstraints.MinimalTransactionAmount, TransactionConstraints.MaximumTransactionAmount);

        RuleFor(x => x.Label)
            .ApplyLength(TransactionConstraints.MaximumLabelLength);
          

        RuleFor(x => x.Note)
            .ApplyLength(TransactionConstraints.MaximumNoteLength);

        RuleFor(x => x.CategoryId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.CategoryRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Category)} with id does not exist [{x.CategoryId}]");

        RuleFor(x => x.CurrencyId)
            .ApplyCommonRules()
            .MustAsync(async (id, _) => await unitOfWork.CurrencyRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id does not exist [{x.CurrencyId}]");
    }
}