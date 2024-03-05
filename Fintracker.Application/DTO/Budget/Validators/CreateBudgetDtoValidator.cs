﻿using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using FluentValidation;

namespace Fintracker.Application.DTO.Budget.Validators;

public class CreateBudgetDtoValidator : AbstractValidator<CreateBudgetDTO>
{

    public CreateBudgetDtoValidator(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        
        Include(new BudgetBaseDtoValidator(unitOfWork));
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage($"{nameof(CreateBudgetDTO.UserId)} can not be blank")
            .NotNull()
            .WithMessage($"{nameof(CreateBudgetDTO.UserId)} must be included")
            .MustAsync((guid, token) => { return userRepository.ExistsAsync(guid); })
            .WithMessage(x => $"User with id [{x}] does not exists");
    }
}