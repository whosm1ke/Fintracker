using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionPureDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public Guid WalletId { get; set; }
    
    public Guid UserId { get; set; }
    
    public decimal Amount { get; set; }

    public string? Note { get; set; }
    
    public string? Label { get; set; }

    public DateTime CreatedAt { get; set; } = default!;

    public bool IsBankTransaction { get; set; }
}