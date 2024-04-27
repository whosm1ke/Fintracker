using Fintracker.Application.Helpers;
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
            .ApplyCommonRules()
            .ApplyEmail()
            .MustAsync(async (email, _) => await userManager.FindByEmailAsync(email) == null)
            .WithMessage("Email already been in use");

        RuleFor(x => x.Password)
            .ApplyCommonRules();

        RuleFor(x => x.ConfirmPassword)
            .ApplyCommonRules()
            .Equal(x => x.Password)
            .WithName("Confirm password")
            .WithMessage("Not equals to password");
        
        RuleFor(x => x.UserName)
            .ApplyCommonRules();
            
    }
}