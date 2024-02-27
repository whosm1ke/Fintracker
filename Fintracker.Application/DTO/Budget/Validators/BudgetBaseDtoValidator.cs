using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class BudgetBaseDtoValidator : AbstractValidator<IBudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BudgetBaseDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;


        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(BudgetConstraints.MinBalance)
            .WithMessage(x =>
                $"Minimal balance should be greater or equal to {BudgetConstraints.MinBalance}")
            .LessThanOrEqualTo(BudgetConstraints.MaxBalance)
            .WithMessage(x =>
                $"Maximum balance should be less or equal to {BudgetConstraints.MaxBalance}")
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.Balance)} can not be blank");

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.Name)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.Name)} can not be blank")
            .MaximumLength(BudgetConstraints.MaximumNameLength)
            .WithMessage(x =>
                $"{nameof(CreateBudgetDTO.Name)} should be less then {BudgetConstraints.MaximumNameLength}")
            .MinimumLength(BudgetConstraints.MinimumNameLength)
            .WithMessage(x =>
                $"{nameof(CreateBudgetDTO.Name)} should be less then {BudgetConstraints.MinimumNameLength}");

        RuleFor(x => x.CurrencyId)
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.CurrencyId)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.CurrencyId)} must be included")
            .MustAsync((guid, token) => { return _unitOfWork.CurrencyRepository.ExistsAsync(guid); })
            .WithMessage(x => $"Currency with id [{x.CurrencyId}] does not exists");

        RuleFor(x => x.WalletId)
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.WalletId)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.WalletId)} must be included")
            .MustAsync((guid, token) => { return _unitOfWork.WalletRepository.ExistsAsync(guid); })
            .WithMessage(x => $"Wallet with id [{x.WalletId}] does not exists");


        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.StartDate)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.StartDate)} must be included")
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start Date should be less than End Date");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.EndDate)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.EndDate)} must be included")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End Date should be greater than Start Date");

        RuleFor(x => x.CategoryIds)
            .MustAsync((guids, token) => ExistInDatabase(guids))
            .WithMessage("One or more CategoryIds do not exist in the database.");
    }

    private async Task<bool> ExistInDatabase(ICollection<Guid> categoryIds)
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsyncNoTracking();
        foreach (var category in categories)
        {
            bool isExists = categoryIds.Any(id => id.Equals(category?.Id));
            if (!isExists) return false;
        }

        return true;
    }
}