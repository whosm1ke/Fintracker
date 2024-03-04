using Fintracker.Domain.Common;
using Fintracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence;

public abstract class AuditableDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AuditableDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual async Task<int> SaveChangesAsync(string username = "SYSTEM")
    {
        foreach (var entry in base.ChangeTracker.Entries<IEntity<Guid>>()
                     .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.ModifiedAt = DateTime.Now;
            entry.Entity.ModifiedBy = username;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
                entry.Entity.CreatedBy = username;
            }
        }

        var result = await base.SaveChangesAsync();

        return result;
    }
}