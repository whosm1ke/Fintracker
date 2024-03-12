using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDTO>
{
    public CreateCategoryDtoValidator()
    {
        Include(new CategoryBaseValidator());


        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .IsInEnum()
            .WithMessage($"Has allowed values: {CategoryTypeEnum.INCOME} or {CategoryTypeEnum.EXPENSE}");
    }
}