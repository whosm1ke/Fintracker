using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Budget;

public class BudgetPureDTO : FinancialEntityDTO
{
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public Guid UserId { get; set; }

    public bool IsPublic { get; set; }
    
    public ICollection<CategoryDTO> Categories { get; set; } = default!;
    
}