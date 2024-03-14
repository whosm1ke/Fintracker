using Fintracker.Application.DTO.Budget;

namespace Fintracker.Application.DTO.User;

public class UserWithBudgetsDTO : UserBaseDTO
{
    public ICollection<BudgetBaseDTO> Budgets { get; set; } = default!;
}