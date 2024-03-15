using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        RuleFor(x => x.Category)
            .SetValidator(new CategoryBaseValidator())
            .OverridePropertyName(string.Empty);

        RuleFor(x => x.Category.Id)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateCategoryCommand.Category.Id))
            .MustAsync(async (guid, _) => await unitOfWork.CategoryRepository.ExistsAsync(guid))
            .WithMessage(x => $"Category with id does not exists [{x.Category.Id}]");
        
        RuleFor(x => x.Category.UserId)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateCategoryCommand.Category.UserId))
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.Category.UserId}]");
    }
}