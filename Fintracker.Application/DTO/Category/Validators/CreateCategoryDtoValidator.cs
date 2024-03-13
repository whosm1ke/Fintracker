using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Category)
            .SetValidator(new CategoryBaseValidator())
            .OverridePropertyName(string.Empty);


        RuleFor(x => x.Category.Type)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateCategoryCommand.Category.Type))
            .IsInEnum()
            .WithMessage($"Has allowed values: {CategoryTypeEnum.INCOME} or {CategoryTypeEnum.EXPENSE}");
    }
}