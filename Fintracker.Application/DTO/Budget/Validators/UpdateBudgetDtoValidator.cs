using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class UpdateBudgetDtoValidator : AbstractValidator<UpdateBudgetDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBudgetDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Include(new BudgetBaseDtoValidator(unitOfWork));
        
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage($"{nameof(UpdateBudgetDTO.Id)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(UpdateBudgetDTO.Id)} must be included")
            .MustAsync((guid, token) => { return _unitOfWork.BudgetRepository.ExistsAsync(guid); })
            .WithMessage(x => $"Budget with id [{x.Id}] does not exists");
            
    }
}