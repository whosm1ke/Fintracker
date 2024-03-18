namespace Fintracker.Application.DTO.Currency;

public class ConvertCurrencyDTO
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public decimal Amount { get; set; }
    public decimal Value { get; set; }
}