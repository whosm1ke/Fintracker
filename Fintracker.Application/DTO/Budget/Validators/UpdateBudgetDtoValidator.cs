using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class UpdateBudgetDtoValidator : AbstractValidator<UpdateBudgetDTO>
{

    public UpdateBudgetDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new BudgetBaseDtoValidator(unitOfWork));
        
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (guid, _) => await unitOfWork.BudgetRepository.ExistsAsync(guid))
            .WithMessage(x => $"Budget with id does not exist [{x.Id}]");
            
    }
}