using FluentValidation;

namespace Fintracker.Application.Helpers;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> ApplyCommonRules<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .WithMessage("Must be included")
            .NotEmpty()
            .WithMessage("Can not be blank")
            .When(x => x is not null);
    }

    public static IRuleBuilderOptions<T, string> ApplyMinMaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int min,
        int max)
    {
        return ruleBuilder
            .ApplyMinLength(min)
            .ApplyMaxLength(max);
    }
    
    public static IRuleBuilderOptions<T, string> ApplyEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(
                @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")
            .WithMessage("Has wrong format");
    }

    public static IRuleBuilderOptions<T, TProperty> ApplyGreaterLessThan<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, TProperty min,
        TProperty max) where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .ApplyGreaterThanEqual(min)
            .ApplyLessThanEqual(max);
    }

    public static IRuleBuilderOptions<T, TProperty> ApplyGreaterThanEqual<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, TProperty min)
        where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(min)
            .WithMessage($"Should be greater than or equal to {min}");
    }
    
    public static IRuleBuilderOptions<T, TProperty> ApplyLessThanEqual<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, TProperty max)
        where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .LessThanOrEqualTo(max)
            .WithMessage($"Should be less than or equal to {max}");
    }

    public static IRuleBuilderOptions<T, string> ApplyMaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int max)
    {
        return ruleBuilder
            .MaximumLength(max)
            .WithMessage($"Maximum length is {max}");
    }

    public static IRuleBuilderOptions<T, string> ApplyMinLength<T>(this IRuleBuilder<T, string> ruleBuilder, int min)
    {
        return ruleBuilder
            .MinimumLength(min)
            .WithMessage($"Minimum length is {min}");
    }
    
    public static IRuleBuilderOptions<T, string?> ApplyLength<T>(this IRuleBuilder<T, string?> ruleBuilder, int max, int min = 0)
    {
        return ruleBuilder
            .Length(min, max)
            .WithMessage($"Is optional, but should be less than {max}");
    }
}