using Fintracker.Domain.Common;

namespace Fintracker.Domain.Entities;

public class Budget : IEntity<Guid>
{

    public Budget()
    {
        Categories = new HashSet<Category>();
    }
    
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; }
    
    public ICollection<Category> Categories { get; set; }
    
    public string Name { get; set; }

    public decimal TotalAmount { get; set; }

    public Guid CurrencyId { get; set; }

    public Currency Currency { get; set; }
    
    public Guid WalletId { get; set; }

    public Wallet Wallet { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

}