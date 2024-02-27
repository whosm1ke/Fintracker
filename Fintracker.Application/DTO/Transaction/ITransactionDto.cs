namespace Fintracker.Application.DTO.Transaction;

public interface ITransactionDto
{
    public Guid CategoryId { get; set; }
    
    public Guid CurrencyId { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }
    
    public string? Label { get; set; }
}