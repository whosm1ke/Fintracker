using Fintracker.Application.DTO.Budget;

namespace Fintracker.Application.DTO.Wallet;

public class WalletWithBudgetsDTO : WalletBaseDTO
{
    public ICollection<BudgetBaseDTO> Budgets { get; set; } = default!;
}