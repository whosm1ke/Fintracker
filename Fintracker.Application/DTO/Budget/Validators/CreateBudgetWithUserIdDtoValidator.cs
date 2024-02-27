using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class CreateBudgetWithUserIdDtoValidator : AbstractValidator<CreateBudgetDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBudgetWithUserIdDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        Include(new BudgetBaseDtoValidator(unitOfWork));
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.UserId)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.UserId)} must be included")
            .MustAsync((guid, token) => { return _unitOfWork.UserRepository.ExistsAsync(guid); })
            .WithMessage(x => $"User with id [{x}] does not exists");
    }
}