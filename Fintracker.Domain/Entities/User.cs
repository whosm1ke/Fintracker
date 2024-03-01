using Fintracker.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Fintracker.Domain.Entities;

public class User: IdentityUser<Guid>, IEntity<Guid>
{
    public User()
    {
        Wallets = new HashSet<Wallet>();
        Budgets = new HashSet<Budget>();
    }
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; }

    public ICollection<Wallet> Wallets { get; set; }
    
    public ICollection<Budget> Budgets { get; set; }

    public UserDetails UserDetails { get; set; }
}

public class UserDetails
{
    public string FName { get; set; }
    
    public string LName { get; set; }

    public string Sex { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Avatar { get; set; }

    public string Language { get; set; }
}