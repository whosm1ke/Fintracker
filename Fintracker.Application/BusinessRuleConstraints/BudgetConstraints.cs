namespace Fintracker.Application.BusinessRuleConstants;

public static class BudgetConstraints
{
    public const decimal MinBalance = 0.01m;
    public const decimal MaxBalance = 100_000_000_000m;
    public const int MaximumNameLength = 50;
}