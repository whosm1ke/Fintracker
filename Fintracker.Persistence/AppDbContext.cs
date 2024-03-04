using Fintracker.Domain.Entities;
using Fintracker.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Fintracker.Persistence;

public class AppDbContext : AuditableDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new EntityConfiguration());
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }


    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
}