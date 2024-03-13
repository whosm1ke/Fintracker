using Fintracker.Application.Helpers;
using Fintracker.Application.Models.Identity;
using Fintracker.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Fintracker.Identity.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator(UserManager<User> userManager)
    {
        RuleFor(x => x.Email)
            .ApplyCommonRules()
            .ApplyEmail()
            .MustAsync(async (email, _) => await userManager.FindByEmailAsync(email) != null)
            .WithMessage("Invalid credentials");

        RuleFor(x => x.Password)
            .MustAsync(async (dto, pass, _) =>
            {
                var user = await userManager.FindByEmailAsync(dto.Email);
                if (user is null) return false;
                return await userManager.CheckPasswordAsync(user, pass);
            })
            .WithMessage("Invalid credentials");
    }
}

