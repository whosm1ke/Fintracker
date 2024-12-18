using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class DeleteCategoryDtoValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(DeleteCategoryCommand.UserId))
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.UserId}]");

        RuleFor(x => x.CategoryToReplaceId)
            .ApplyCommonRules(x => x.CategoryToReplaceId.HasValue)
            .OverridePropertyName(nameof(DeleteCategoryCommand.CategoryToReplaceId))
            .MustAsync(async (dto, catToReplace, _) =>
                await unitOfWork.CategoryRepository.ExistsAsync(catToReplace.Value) && catToReplace != dto.Id)
            .WithMessage(x => $"{nameof(Domain.Entities.Category)} with id does not exist [{x.CategoryToReplaceId}]")
            .When(x => x.CategoryToReplaceId.HasValue);
    }
}