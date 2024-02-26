namespace Fintracker.Application.DTO.Budget;

public class CreateBudgetDTO
{
    public string Name { get; set; }

    public decimal TotalAmount { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public ICollection<Guid> CategoryIds { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}