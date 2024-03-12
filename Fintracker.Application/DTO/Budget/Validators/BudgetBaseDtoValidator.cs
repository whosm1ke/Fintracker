using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class BudgetBaseDtoValidator : AbstractValidator<IBudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BudgetBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;


        RuleFor(x => x.Balance)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .GreaterThanOrEqualTo(BudgetConstraints.MinBalance)
            .WithMessage(x =>
                $"Minimal balance should be greater or equal to {BudgetConstraints.MinBalance}")
            .LessThanOrEqualTo(BudgetConstraints.MaxBalance)
            .WithMessage(x =>
                $"Maximum balance should be less or equal to {BudgetConstraints.MaxBalance}");

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MaximumLength(BudgetConstraints.MaximumNameLength)
            .WithMessage(x =>
                $"Should be less then {BudgetConstraints.MaximumNameLength}")
            .MinimumLength(BudgetConstraints.MinimumNameLength)
            .WithMessage(x =>
                $"Should be less then {BudgetConstraints.MinimumNameLength}");

        RuleFor(x => x.CurrencyId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (guid, _) => await _unitOfWork.CurrencyRepository.ExistsAsync(guid))
            .WithMessage(x => $"Does not exists [{x.CurrencyId}]");

        RuleFor(x => x.WalletId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (guid, _) => await _unitOfWork.WalletRepository.ExistsAsync(guid))
            .WithMessage(x => $"Does not exists [{x.WalletId}]");


        RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Should be less than End Date");

        RuleFor(x => x.EndDate)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Should be greater than Start Date");

        RuleFor(x => x.CategoryIds)
            .MustAsync(async (guids, _) => await ExistInDatabase(guids))
            .WithMessage("One or more do not exist.");
    }

    private async Task<bool> ExistInDatabase(ICollection<Guid> categoryIds)
    {
        int existCategoriesCounter = 0;
        var categories = await _unitOfWork.CategoryRepository.GetAllAsyncNoTracking();
        foreach (var category in categories)
        {
            bool isExists = categoryIds.Any(id => id.Equals(category?.Id));
            if (isExists) existCategoriesCounter++;
        }

        return existCategoriesCounter == categoryIds.Count;
    }
}