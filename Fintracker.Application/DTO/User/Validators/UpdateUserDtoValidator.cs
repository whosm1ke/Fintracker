using Fintracker.Application.Contracts.Identity;
using FluentValidation;

namespace Fintracker.Application.DTO.User.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.Id}]");

        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .MustAsync(async (dto, email, _) =>
            {
                var existingUser = await userRepository.GetAsNoTrackingAsync(email);

                if (existingUser is null)
                    return true;
                if (existingUser.Email == dto.Email && existingUser.Id == dto.Id)
                    return true;
                return false;
            })
            .WithMessage(x => $"Invalid email [{x.Email}]");

        RuleFor(x => x.UserDetails).SetValidator(new UserDetailsValidator());
    }
}