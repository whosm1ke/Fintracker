using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Helpers;
using FluentValidation;
using Microsoft.Extensions.Options;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Fintracker.Application.DTO.User.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserDtoValidator(IUserRepository userRepository, IOptions<AppSettings> options, IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.User.Id)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateUserCommand.User.Id))
            .MustAsync(async (id, _) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.User)} with id does not exist [{x.User.Id}]");

        RuleFor(x => x.User.CurrencyId)
            .ApplyCommonRules()
            .OverridePropertyName(nameof(UpdateUserCommand.User.CurrencyId))
            .MustAsync(async (id, _) => await unitOfWork.CurrencyRepository.ExistsAsync(id))
            .WithMessage(x => $"{nameof(Domain.Entities.Currency)} with id does not exist [{x.User.CurrencyId}]");
        
        RuleFor(x => x.User.UserDetails)
            .SetValidator(new UserDetailsValidator(options))
            .OverridePropertyName(string.Empty);
    }
}