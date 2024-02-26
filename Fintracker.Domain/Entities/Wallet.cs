using Fintracker.Domain.Common;

namespace Fintracker.Domain.Entities;

public class Wallet : IEntity<Guid>
{

    public Wallet()
    {
        Users = new HashSet<User>();
        Transactions = new HashSet<Transaction>();
        Budgets = new HashSet<Budget>();
    }
    
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; }

    public ICollection<User> Users { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public ICollection<Budget> Budgets { get; set; }

    public string Name { get; set; }

    public decimal StartBalance { get; set; }
    
    public decimal TotalSpent { get; set; }

    public Guid CurrencyId { get; set; }
}