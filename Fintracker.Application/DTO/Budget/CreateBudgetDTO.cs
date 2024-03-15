namespace Fintracker.Application.DTO.Budget;

public class CreateBudgetDTO : IBudgetDto
{
    public string Name { get; set; } = default!;

    public decimal Balance { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public Guid WalletId { get; set; }
    
    public ICollection<Guid> CategoryIds { get; set; } = default!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsPublic { get; set; }

    public Guid UserId { get; set; }
    
}