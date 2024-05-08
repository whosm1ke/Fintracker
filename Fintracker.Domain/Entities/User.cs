using Fintracker.Domain.Common;
using Fintracker.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Fintracker.Domain.Entities;

public class User : IdentityUser<Guid>, IEntity<Guid>
{
    public User()
    {
        OwnedWallets = new HashSet<Wallet>();
        MemberWallets = new HashSet<Wallet>();
        OwnedBudgets = new HashSet<Budget>();
        MemberBudgets = new HashSet<Budget>();
        Categories = new HashSet<Category>();
    }

    public ICollection<Wallet> OwnedWallets { get; set; }
    public ICollection<Wallet> MemberWallets { get; set; }

    public ICollection<Category> Categories { get; set; }
    public ICollection<Budget> OwnedBudgets { get; set; }

    public ICollection<Budget> MemberBudgets { get; set; }

    public UserDetails UserDetails { get; set; }
    
    public Guid UserDetailsId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime ModifiedAt { get; set; }
    public string ModifiedBy { get; set; } = default!;

    public Currency GlobalCurrency { get; set; }
    public Guid CurrencyId { get; set; }
}

public class UserDetails
{
    public Guid Id { get; set; }

    public string? Sex { get; set; } = default!;

    public DateTime? DateOfBirth { get; set; }

    public string? Avatar { get; set; } = default!;

    public Language Language { get; set; }
}