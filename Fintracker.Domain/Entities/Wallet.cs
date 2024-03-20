using Fintracker.Domain.Common;

namespace Fintracker.Domain.Entities;

public class Wallet : FinancialEntity
{
    public Wallet()
    {
        Users = new HashSet<User>();
        Transactions = new HashSet<Transaction>();
        Budgets = new HashSet<Budget>();
    }

    public User Owner { get; set; } = default!;
    public Guid OwnerId { get; set; }

    public ICollection<User> Users { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public ICollection<Budget> Budgets { get; set; }
    

    public bool IsBanking { get; set; }

    public string? BankAccountId { get; set; } = default!;
}