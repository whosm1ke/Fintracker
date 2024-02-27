using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDTO>
{
    public CreateCategoryDtoValidator()
    {
        Include(new CategoryBaseValidator());

        string allowedTypeValues = string.Join(',', Enum.GetNames<CategoryTypeEnum>());

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage($"{nameof(CreateCategoryDTO.Type)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(CreateCategoryDTO.Type)} can not be blank")
            .IsInEnum()
            .WithMessage($"{nameof(CreateCategoryDTO.Type)} has allowed values: {allowedTypeValues}");
    }
}