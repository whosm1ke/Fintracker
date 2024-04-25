using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionBaseDTO : TransactionPureDTO
{

    public ICollection<BudgetPureDTO> Budgets { get; set; }
    
    public UserPureDTO User { get; set; } = default!;
    
    public WalletPureDTO Wallet { get; set; } = default!;
    
}