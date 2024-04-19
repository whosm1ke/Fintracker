using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.User;

public class UserBaseDTO : UserPureDTO
{
    public ICollection<WalletPureDTO> OwnedWallets { get; set; } = default!;
    
    public ICollection<WalletPureDTO> MemberWallets { get; set; } = default!;
    
    public ICollection<BudgetPureDTO> Budgets { get; set; } = default!;
    
}