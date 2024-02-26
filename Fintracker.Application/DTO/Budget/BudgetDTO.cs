using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Budget;

public class BudgetDTO : IBaseDto
{
    public Guid Id { get; set; }

    public ICollection<CategoryDTO> Categories { get; set; }
    
    public string Name { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal TotalSpent { get; set; }

    public Guid CurrencyId { get; set; }
    
    public Guid WalletId { get; set; }

    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}