using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Application.Helpers;
using FluentValidation;

namespace Fintracker.Application.DTO.User.Validators;

public class UserDetailsValidator : AbstractValidator<UserDetailsDTO>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

    public UserDetailsValidator()
    {
        RuleFor(x => x.Avatar)
            .ApplyLength(UserDetailsConstraints.MaxAvatarLength)
            .Must(BeAValidExtension)
            .When(x => !string.IsNullOrEmpty(x.Avatar))
            .WithMessage($"Allowed file extensions: {string.Join(", ", _allowedExtensions)}");

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
        return _allowedExtensions.Contains(extension);
    }
}