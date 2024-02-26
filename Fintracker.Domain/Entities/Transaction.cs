using Fintracker.Domain.Common;

namespace Fintracker.Domain.Entities;

public class Transaction : IEntity<Guid>
{
    public Transaction()
    {
        Budgets = new HashSet<Budget>();
    }
    
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; }

    public Guid WalletId { get; set; }

    public Wallet Wallet { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }

    public ICollection<Budget> Budgets { get; set; }
    
    public Guid CurrencyId { get; set; }

    public Currency Currency { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }
    
    public string? Label { get; set; }
    
}