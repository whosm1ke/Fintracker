using Fintracker.Domain.Common;
using Fintracker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(
        options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        List<IdentityRole<Guid>> roles = new()
        {
            new IdentityRole<Guid>()
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = new Guid("DEEBA8F0-E75F-46F4-A447-68F5341FDBEC")
            },
            new IdentityRole<Guid>()
            {
                Name = "User",
                NormalizedName = "USER",
                Id = new Guid("33A23BC4-BD7E-4C8B-887B-8787F3E13614")
            }
        };

        builder.Entity<IdentityRole<Guid>>().HasData(roles);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var username = _httpContextAccessor.HttpContext.User.FindFirst("Uid")?.Value;
        
        foreach (var entry in base.ChangeTracker.Entries<IEntity<Guid>>()
                     .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.ModifiedAt = DateTime.Now;
            entry.Entity.ModifiedBy = username ?? "SYSTEM";

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
                entry.Entity.CreatedBy = username ?? "SYSTEM";
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
}