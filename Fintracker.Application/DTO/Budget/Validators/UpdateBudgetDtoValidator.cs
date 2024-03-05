using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class UpdateBudgetDtoValidator : AbstractValidator<UpdateBudgetDTO>
{

    public UpdateBudgetDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new BudgetBaseDtoValidator(unitOfWork));
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateBudgetDTO.Id)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(UpdateBudgetDTO.Id)} must be included")
            .MustAsync(async (guid, token) => { return await unitOfWork.BudgetRepository.ExistsAsync(guid); })
            .WithMessage(x => $"Budget with id [{x.Id}] does not exists");
            
    }
}