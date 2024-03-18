namespace Fintracker.Application.DTO.Transaction;

public class CreateTransactionDTO : ITransactionDto
{
    public Guid WalletId { get; set; }
    
    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }
    
    public Guid CurrencyId { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }
    
    public string? Label { get; set; }

    public DateTime CreatedAt { get => DateTime.Now; }
}