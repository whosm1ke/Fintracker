using Fintracker.Application.DTO.Budget;

namespace Fintracker.Application.DTO.Wallet;

public class WalletWithBudgetsDTO : WalletPureDTO
{
    public ICollection<BudgetBaseDTO> Budgets { get; set; } = default!;
}