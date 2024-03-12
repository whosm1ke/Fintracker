using Fintracker.Application.BusinessRuleConstraints;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class CategoryBaseValidator : AbstractValidator<ICategoryDto>
{
    public CategoryBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MinimumLength(CategoryConstraints.MinimumNameLength)
            .WithMessage($"Minimum length is {CategoryConstraints.MinimumNameLength}")
            .MaximumLength(CategoryConstraints.MaximumNameLength)
            .WithMessage($"Maximum length is {CategoryConstraints.MaximumNameLength}");

        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MaximumLength(CategoryConstraints.MaximumImageLength)
            .WithMessage($"Maximum character length is {CategoryConstraints.MaximumImageLength}");

        RuleFor(x => x.IconColour)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MaximumLength(CategoryConstraints.MaximumIconColourLength)
            .WithMessage($"Maximum character length is {CategoryConstraints.MaximumIconColourLength}");
    }
}