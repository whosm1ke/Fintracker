namespace Fintracker.Application.BusinessRuleConstants;

public static class WalletConstraints
{
    public const decimal MaxBalance = 100_000_000_000m;
    public const int MaxNameLength = 50;
    public const int MinNameLength = 1;
}