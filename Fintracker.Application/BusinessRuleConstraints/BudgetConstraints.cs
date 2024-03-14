namespace Fintracker.Application.BusinessRuleConstants;

public static class BudgetConstraints
{
    public const decimal MinBalance = 0.01m;
    public const decimal MaxBalance = 100_000_000_000m;
    public const int MaximumNameLength = 50;
    public const int MinimumNameLength = 1;
}

public static class UserDetailsConstraints
{
    public const int MaxAvatarLength = 250;
    public const int MaxNameLength = 60;
    public const int MaxSexLength = 20;
}