using Fintracker.Domain.Common;

namespace Fintracker.Domain.Entities;

public class Budget : FinancialEntity
{
    public Budget()
    {
        Categories = new HashSet<Category>();
    }

    public ICollection<Category> Categories { get; set; }
    

    public User User { get; set; } = default!;

    public Guid UserId { get; set; }

    public Wallet Wallet { get; set; } = default!;

    public Guid WalletId { get; set; }
    

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsPublic { get; set; }
}