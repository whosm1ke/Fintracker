using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDTO>
{
    public UpdateCategoryDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new CategoryBaseValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateCategoryDTO.Id)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(UpdateCategoryDTO.Id)} must be included")
            .MustAsync((guid, token) => { return unitOfWork.CategoryRepository.ExistsAsync(guid); })
            .WithMessage(x => $"Category with id [{x.Id}] does not exists");
    }
}