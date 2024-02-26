using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Budget;

public class BudgetWithUserDTO : BudgetBaseDTO
{
    public UserBaseDTO User { get; set; }
}