using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Budget;

public class UpdateBudgetDTO : CreateBudgetDTO, IBaseDto
{
    public Guid Id { get; set; }
}