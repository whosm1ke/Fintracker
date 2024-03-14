using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Application.Helpers;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.DTO.User.Validators;

public class UserDetailsValidator : AbstractValidator<UserDetailsDTO>
{
    private readonly AppSettings _appSettings;
    public UserDetailsValidator(IOptions<AppSettings> options)
    {
        _appSettings = options.Value;
        RuleFor(x => x.Avatar)
            .ApplyLength(UserDetailsConstraints.MaxAvatarLength)
            .Must(BeAValidExtension)
            .When(x => x.Avatar is not null)
            .WithMessage($"Allowed file extensions: {string.Join(", ", _appSettings.AllowedExtensions)}");

        RuleFor(x => x.DateOfBirth)
            .GreaterThan(new DateTime(1915, 1, 1))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Must be greater than 1915-01-01");

        RuleFor(x => x.FName)
            .ApplyLength(UserDetailsConstraints.MaxNameLength);

        RuleFor(x => x.LName)
            .ApplyLength(UserDetailsConstraints.MaxNameLength);

        RuleFor(x => x.Language)
            .IsInEnum()
            .When(x => x.Language.HasValue)
            .WithMessage(
                $"Allowed languages are: {LanguageTypeEnum.English}, {LanguageTypeEnum.Ukrainian}, " +
                $"{LanguageTypeEnum.Deutch}");

        RuleFor(x => x.Sex)
            .ApplyLength(UserDetailsConstraints.MaxSexLength);

    }

    private bool BeAValidExtension(string? avatar)
    {
        if (string.IsNullOrEmpty(avatar))
            return true;
        var extension = Path.GetExtension(avatar).ToLower();
        return _appSettings.AllowedExtensions.Contains(extension);
    }
}