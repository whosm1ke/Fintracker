using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Category.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Category)
            .SetValidator(new CategoryBaseValidator())
            .OverridePropertyName(string.Empty);


        RuleFor(x => x.Category.Type)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateCategoryCommand.Category.Type))
            .IsInEnum()
            .WithMessage($"Has allowed values: {CategoryTypeEnum.INCOME} or {CategoryTypeEnum.EXPENSE}");

        RuleFor(x => x.UserId)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(CreateCategoryCommand.UserId))
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.UserId}]");
    }
}