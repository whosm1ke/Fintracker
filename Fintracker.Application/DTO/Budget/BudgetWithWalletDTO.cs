using Fintracker.Application.DTO.Wallet;

namespace Fintracker.Application.DTO.Budget;

public class BudgetWithWalletDTO : BudgetBaseDTO
{
    public WalletBaseDTO WalletBase { get; set; }
}