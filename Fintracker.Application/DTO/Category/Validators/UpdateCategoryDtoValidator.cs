using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDTO>
{
    public UpdateCategoryDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new CategoryBaseValidator());

        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (guid, _) => await unitOfWork.CategoryRepository.ExistsAsync(guid))
            .WithMessage(x => $"Category with id does not exists [{x.Id}]");
    }
}