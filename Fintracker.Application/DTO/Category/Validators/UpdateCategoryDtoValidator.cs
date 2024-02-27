using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCategoryDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Include(new CategoryBaseValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateCategoryDTO.Id)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(UpdateCategoryDTO.Id)} must be included")
            .MustAsync((guid, token) => { return _unitOfWork.CategoryRepository.ExistsAsync(guid); })
            .WithMessage(x => $"Category with id [{x.Id}] does not exists");
    }
}