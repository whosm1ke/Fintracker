namespace Fintracker.Application.Models.Monobank;

public class MonobankConfiguration
{
    public string AccountId { get; set; } = default!;
    public long From { get; set; }
    public long? To { get; set; }
}