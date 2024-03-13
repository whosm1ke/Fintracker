using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class CategoryBaseValidator : AbstractValidator<ICategoryDto>
{
    public CategoryBaseValidator()
    {
        RuleFor(x => x.Name)
            .ApplyCommonRules()
            .ApplyMinMaxLength(CategoryConstraints.MinimumNameLength, CategoryConstraints.MaximumNameLength);

        RuleFor(x => x.Image)
            .ApplyCommonRules()
            .ApplyMaxLength(CategoryConstraints.MaximumImageLength);

        RuleFor(x => x.IconColour)
            .ApplyCommonRules()
            .ApplyMaxLength(CategoryConstraints.MaximumIconColourLength);
    }
}