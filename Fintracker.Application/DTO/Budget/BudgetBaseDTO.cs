using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.Budget;

public class BudgetBaseDTO : FinancialEntityDTO
{

    public ICollection<CategoryDTO> Categories { get; set; } = default!;
    
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public Guid UserId { get; set; }

    public bool IsPublic { get; set; }

}