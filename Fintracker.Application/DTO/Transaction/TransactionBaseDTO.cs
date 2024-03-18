using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionBaseDTO : TransactionPureDTO
{
    public CategoryDTO Category { get; set; } = default!;
    
    public CurrencyDTO Currency { get; set; } = default!;
}