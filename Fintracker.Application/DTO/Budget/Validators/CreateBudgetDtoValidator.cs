using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class CreateBudgetDtoValidator : AbstractValidator<CreateBudgetDTO>
{

    public CreateBudgetDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        
        Include(new BudgetBaseDtoValidator(unitOfWork));
        
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (guid, _) => await userRepository.ExistsAsync(guid))
            .WithMessage(x => $"Does not exist [{x.UserId}]");
    }
}