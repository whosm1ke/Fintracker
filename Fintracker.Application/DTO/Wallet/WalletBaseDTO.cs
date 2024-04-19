using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Wallet;

public class WalletBaseDTO : WalletPureDTO
{
    public UserBaseDTO Owner { get; set; } = default!;
    
    public ICollection<BudgetPureDTO> Budgets { get; set; } = default!;
    
    public ICollection<UserPureDTO> Users { get; set; } = default!;
    
    public ICollection<TransactionPureDTO> Transactions { get; set; } = default!;
}