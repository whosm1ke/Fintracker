using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Helpers;
using FluentValidation;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Fintracker.Application.DTO.Budget.Validators;

public class BudgetBaseDtoValidator : AbstractValidator<IBudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BudgetBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;


        RuleFor(x => x.StartBalance)
            .NotEmpty()
            .ApplyCommonRules()
            .ApplyGreaterLessThan(BudgetConstraints.MinBalance, BudgetConstraints.MaxBalance);


        RuleFor(x => x.Name)
            .ApplyCommonRules(x => x.Name is not null)
            .ApplyMinMaxLength(BudgetConstraints.MinimumNameLength, BudgetConstraints.MaximumNameLength);

        RuleFor(x => x.CurrencyId)
            .ApplyCommonRules()
            .MustAsync(async (guid, _) => await _unitOfWork.CurrencyRepository.ExistsAsync(guid))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id does not exist [{x.CurrencyId}]");
        
        RuleFor(x => x.WalletId)
            .ApplyCommonRules()
            .MustAsync(async (guid, _) => await _unitOfWork.WalletRepository.ExistsAsync(guid))
            .WithMessage(x => $"{nameof(Domain.Entities.Wallet)} with id does not exist [{x.WalletId}]");
        
        
        RuleFor(x => x.StartDate)
            .ApplyCommonRules()
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Should be less than End Date");
        
        RuleFor(x => x.EndDate)
            .ApplyCommonRules()
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Should be greater than Start Date");
        
        RuleFor(x => x.CategoryIds)
            .ApplyCommonRules(x => x.CategoryIds is not null)
            .MustAsync(async (guids, _) => await ExistInDatabase(guids))
            .WithMessage("One or more category(ies) do not exist.");

        RuleFor(x => x.IsPublic)
            .ApplyCommonRules();
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