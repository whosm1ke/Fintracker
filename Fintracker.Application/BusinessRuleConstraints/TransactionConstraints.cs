namespace Fintracker.Application.BusinessRuleConstants;

public class TransactionConstraints
{
    public const decimal MinimalTransactionAmount = 0.01m;
    public const int MaximumLabelLength = 15;
    public const int MaximumNoteLength = 80;
}