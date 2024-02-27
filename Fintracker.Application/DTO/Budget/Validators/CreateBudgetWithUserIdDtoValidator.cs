using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class CreateBudgetWithUserIdDtoValidator : AbstractValidator<CreateBudgetDTO>
{

    public CreateBudgetWithUserIdDtoValidator(IUnitOfWork unitOfWork)
    {
        
        Include(new BudgetBaseDtoValidator(unitOfWork));
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.UserId)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.UserId)} must be included")
            .MustAsync((guid, token) => { return unitOfWork.UserRepository.ExistsAsync(guid); })
            .WithMessage(x => $"User with id [{x}] does not exists");
    }
}