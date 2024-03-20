using Fintracker.Domain.Entities;

namespace Fintracker.Domain.Common;

public class FinancialEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; } = default!;
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; } = default!;

    public string Name { get; set; } = default!;

    public decimal Balance { get; set; }
    
    public decimal TotalSpent { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public Currency Currency { get; set; } = default!;
}