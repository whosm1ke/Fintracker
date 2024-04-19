using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionBaseDTO : TransactionPureDTO
{
    public CategoryDTO Category { get; set; } = default!;
    
    public CurrencyDTO Currency { get; set; } = default!;
    
    public UserBaseDTO User { get; set; } = default!;
    
    public WalletBaseDTO Wallet { get; set; } = default!;
}