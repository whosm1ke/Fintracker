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
            .WithMessage($"{nameof(ITransactionDto.Amount)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(ITransactionDto.Amount)} can not be blank")
            .GreaterThan(TransactionConstraints.MinimalTransactionAmount)
            .WithMessage(
                $"{nameof(ITransactionDto.Amount)} should be greater than {TransactionConstraints.MinimalTransactionAmount}");

        RuleFor(x => x.Label)
            .Length(0, TransactionConstraints.MaximumLabelLength)
            .WithMessage(
                $"{nameof(ITransactionDto.Label)} is optional, but should be less than {TransactionConstraints.MaximumLabelLength}");

        RuleFor(x => x.Note)
            .Length(0, TransactionConstraints.MaximumNoteLength)
            .WithMessage(
                $"{nameof(ITransactionDto.Note)} is optional, but should be less than {TransactionConstraints.MaximumNoteLength}");

        RuleFor(x => x.CategoryId)
            .NotNull()
            .WithMessage($"{nameof(ITransactionDto.CategoryId)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(ITransactionDto.CategoryId)} can not be blank")
            .MustAsync((id, _) => unitOfWork.CategoryRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Category)} with id [{x.CategoryId}] does not exists");

        RuleFor(x => x.CurrencyId)
            .NotNull()
            .WithMessage($"{nameof(ITransactionDto.CurrencyId)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(ITransactionDto.CurrencyId)} can not be blank")
            .MustAsync((id, _) => unitOfWork.CurrencyRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id [{x.CurrencyId}] does not exists");
    }
}