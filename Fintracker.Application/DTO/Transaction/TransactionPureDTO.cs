using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionPureDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public Guid WalletId { get; set; }
    
    public Guid UserId { get; set; }
    
    public decimal Amount { get; set; }

    public string? Note { get; set; }
    
    public string? Label { get; set; }

    public DateTime Date { get; set; }

    public bool IsBankTransaction { get; set; }
    
    public CategoryDTO Category { get; set; } = default!;
    
    public CurrencyDTO Currency { get; set; } = default!;
}