namespace Fintracker.Application.BusinessRuleConstraints;

public class TransactionConstraints
{
    public const decimal MinimalTransactionAmount = 0.01m;
    public const decimal MaximumTransactionAmount = 100_000_000_000m;
    public const int MaximumLabelLength = 15;
    public const int MaximumNoteLength = 80;
}