using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public Guid WalletId { get; set; }
    
    public Guid UserId { get; set; }

    public CategoryDTO Category { get; set; }
    
    public CurrencyDTO Currency { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }
    
    public string? Label { get; set; }
}