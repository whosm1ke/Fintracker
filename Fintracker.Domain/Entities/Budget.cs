using Fintracker.Domain.Common;

namespace Fintracker.Domain.Entities;

public class
    Budget : IEntity<Guid>
{
    public Budget()
    {
        Categories = new HashSet<Category>();
    }

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime ModifiedAt { get; set; }

    public string ModifiedBy { get; set; } = default!;

    public ICollection<Category> Categories { get; set; }

    public string Name { get; set; } = default!;

    public decimal Balance { get; set; }

    public User User { get; set; } = default!;

    public Guid UserId { get; set; }

    public Wallet Wallet { get; set; } = default!;

    public Guid WalletId { get; set; }

    public Guid CurrencyId { get; set; }

    public Currency Currency { get; set; } = default!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsPublic { get; set; }
}