using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Category)
            .SetValidator(new CategoryBaseValidator())
            .OverridePropertyName(string.Empty);

        RuleFor(x => x.Category.Id)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateCategoryCommand.Category.Id))
            .MustAsync(async (guid, _) => await unitOfWork.CategoryRepository.ExistsAsync(guid))
            .WithMessage(x => $"Category with id does not exists [{x.Category.Id}]");
    }
}