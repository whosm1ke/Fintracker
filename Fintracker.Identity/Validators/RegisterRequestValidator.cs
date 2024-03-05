using Fintracker.Application.DTO.Account;
using Fintracker.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Fintracker.Identity.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(UserManager<User> userManager)
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage($"{nameof(RegisterRequest.Email)} can not be blank")
            .NotEmpty()
            .WithMessage($"{nameof(RegisterRequest.Email)} can not be empty")
            .Matches(
                @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")
            .WithMessage($"{nameof(RegisterRequest.Email)} has wrong format")
            .MustAsync(async (email, _) => { return await userManager.FindByEmailAsync(email) == null; })
            .WithMessage(x => $"User with email {x.Email} already exists");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage($"{nameof(RegisterRequest.Password)} can not be blank")
            .NotEmpty()
            .WithMessage($"{nameof(RegisterRequest.Password)} can not be empty");

        RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .WithName("Confirm password")
            .WithMessage($"{nameof(RegisterRequest.ConfirmPassword)} can not be blank")
            .NotEmpty()
            .WithName("Confirm password")
            .WithMessage($"{nameof(RegisterRequest.ConfirmPassword)} can not be empty")
            .Equal(x => x.Password)
            .WithName("Confirm password")
            .WithMessage("Confirm password does not equal to password");
        
        RuleFor(x => x.UserName)
            .NotNull()
            .WithMessage("Username")
            .WithMessage($"{nameof(RegisterRequest.UserName)} can not be blank")
            .NotEmpty()
            .WithMessage("Username")
            .WithMessage($"{nameof(RegisterRequest.UserName)} can not be empty");
    }
}