namespace Fintracker.Application.DTO.Monobank;

public class MonoTransactionDTO
{
    public long Time { get; set; }

    public string Description { get; set; } = default!;

    public decimal Amount { get; set; }

    public string Comment { get; set; } = default!;
}