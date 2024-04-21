using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.Budget;

public class BudgetBaseDTO : BudgetPureDTO
{
    
    public WalletPureDTO Wallet { get; set; } = default!;
    
    public UserPureDTO User { get; set; } = default!;
}