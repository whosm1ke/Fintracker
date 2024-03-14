using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Helpers;
using FluentValidation;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Fintracker.Application.DTO.Category.Validators;

public class CategoryBaseValidator : AbstractValidator<ICategoryDto>
{
    public CategoryBaseValidator()
    {
        RuleFor(x => x.Name)
            .ApplyCommonRules(x => x.Name is not null)
            .ApplyMinMaxLength(CategoryConstraints.MinimumNameLength, CategoryConstraints.MaximumNameLength);

        RuleFor(x => x.Image)
            .ApplyCommonRules(x => x.Image is not null)
            .ApplyMaxLength(CategoryConstraints.MaximumImageLength);

        RuleFor(x => x.IconColour)
            .ApplyCommonRules(x => x.IconColour is not null)
            .ApplyMaxLength(CategoryConstraints.MaximumIconColourLength);
    }
}