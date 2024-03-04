using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.Property(x => x.Owner)
            .IsRequired();

        builder.HasOne(x => x.Owner)
            .WithMany(x => x.Wallets)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Wallets)
            .UsingEntity(j => j.ToTable("WalletMembers"));

        builder.HasMany(x => x.Transactions)
            .WithOne(x => x.Wallet)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Budgets)
            .WithOne(x => x.Wallet)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(WalletConstraints.MaxNameLength);

        builder.Property(x => x.Currency)
            .IsRequired();

        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable("Wallets",
            t =>
            {
                t.HasCheckConstraint("CK_Wallets_Balance",
                    $"Balance <= {WalletConstraints.MaxBalance}");
            });
    }
}