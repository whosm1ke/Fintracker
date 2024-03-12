using Fintracker.Application.Models.Identity;
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
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .Matches(
                @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")
            .WithMessage("Has wrong format")
            .MustAsync(async (email, _) => await userManager.FindByEmailAsync(email) == null)
            .WithMessage("Already been in use");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank");

        RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .Equal(x => x.Password)
            .WithName("Confirm password")
            .WithMessage("Not equals to password");
        
        RuleFor(x => x.UserName)
            .NotNull()
            .WithMessage("Username")
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Username")
            .WithMessage("Can not be blank");
            
    }
}