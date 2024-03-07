using Fintracker.Application.BusinessRuleConstraints;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class CategoryBaseValidator : AbstractValidator<ICategoryDto>
{
    public CategoryBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage($"{nameof(ICategoryDto.Name)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(ICategoryDto.Name)} can not be blank")
            .MinimumLength(CategoryConstraints.MinimumNameLength)
            .WithMessage($"Minimum length for {nameof(ICategoryDto.Name)} is {CategoryConstraints.MinimumNameLength}")
            .MaximumLength(CategoryConstraints.MaximumNameLength)
            .WithMessage($"Maximum length for {nameof(ICategoryDto.Name)} is {CategoryConstraints.MaximumNameLength}");

        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage($"{nameof(ICategoryDto.Image)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(ICategoryDto.Image)} can not be blank")
            .MaximumLength(CategoryConstraints.MaximumImageLength)
            .WithMessage(
                $"Maximum character length for {nameof(ICategoryDto.Image)} is {CategoryConstraints.MaximumImageLength}");

        RuleFor(x => x.IconColour)
            .NotNull()
            .WithMessage($"{nameof(ICategoryDto.IconColour)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(ICategoryDto.IconColour)} can not be blank")
            .MaximumLength(CategoryConstraints.MaximumIconColourLength)
            .WithMessage(
                $"Maximum character length for {nameof(ICategoryDto.IconColour)} is {CategoryConstraints.MaximumIconColourLength}");
    }
}