using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class UpdateBudgetDtoValidator : AbstractValidator<UpdateBudgetCommand>
{

    public UpdateBudgetDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Budget)
            .SetValidator(new BudgetBaseDtoValidator(unitOfWork))
            .OverridePropertyName(string.Empty);
        
        RuleFor(x => x.Budget.Id)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateBudgetCommand.Budget.Id))
            .MustAsync(async (guid, _) => await unitOfWork.BudgetRepository.ExistsAsync(guid))
            .WithMessage(x => $"Budget with id does not exist [{x.Budget.Id}]");
            
    }
}