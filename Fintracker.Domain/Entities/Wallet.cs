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
    
    public string CreatedBy { get; set; } = default!;
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; } = default!;

    public User Owner { get; set; } = default!;
    public Guid OwnerId { get; set; }

    public ICollection<User> Users { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public ICollection<Budget> Budgets { get; set; }

    public string Name { get; set; } = default!;

    public decimal Balance { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public Currency Currency { get; set; } = default!;

    public bool IsBanking { get; set; }

    public string BankAccountId { get; set; } = default!;
}