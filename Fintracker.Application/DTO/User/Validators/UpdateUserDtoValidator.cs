using Fintracker.Application.Contracts.Identity;
using FluentValidation;

namespace Fintracker.Application.DTO.User.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage($"{nameof(UpdateUserDTO.Id)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(UpdateUserDTO.Id)} can not be blank")
            .MustAsync((id, _) => userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id [{x.Id}] does not exists");
        
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage($"{nameof(UpdateUserDTO.Email)} must be included")
            .NotEmpty()
            .WithMessage($"{nameof(UpdateUserDTO.Email)} can not be blank")
            .MustAsync(async (dto, email,_) =>
            {
                var existingUser = await userRepository.GetAsNoTrackingAsync(email);

                if (existingUser is null)
                    return true;
                if (existingUser.Email == dto.Email && existingUser.Id == dto.Id)
                    return true;
                return false;            })
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with email [{x.Email}] already exists");
        
        //TODO: add rules for user details
    }
}