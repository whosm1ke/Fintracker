using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class CreateBudgetDtoValidator : AbstractValidator<CreateBudgetCommand>
{

    public CreateBudgetDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        RuleFor(x => x.Budget)
            .SetValidator(new BudgetBaseDtoValidator(unitOfWork))
            .OverridePropertyName(string.Empty);
        
        RuleFor(x => x.Budget.UserId)
            .ApplyCommonRules()
            .MustAsync(async (guid, _) => await userRepository.ExistsAsync(guid))
            .OverridePropertyName(nameof(CreateBudgetCommand.Budget.UserId))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.Budget.UserId}]");
    }
}