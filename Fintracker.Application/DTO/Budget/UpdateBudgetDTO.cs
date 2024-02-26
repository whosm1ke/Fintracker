using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Budget;

public class UpdateBudgetDTO :  IBaseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public decimal Balance { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public Guid WalletId { get; set; }
    
    public ICollection<Guid> CategoryIds { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}