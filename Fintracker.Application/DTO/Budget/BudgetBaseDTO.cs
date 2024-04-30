using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.Budget;

public class BudgetBaseDTO : BudgetPureDTO
{
    public ICollection<TransactionPureDTO> Transactions { get; set; }
    
    public WalletPureDTO Wallet { get; set; } = default!;
    
    public UserPureDTO Owner { get; set; } = default!;
    public ICollection<UserPureDTO> Members { get; set; } = default!;
}