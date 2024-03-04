using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(x => x.User)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Wallet)
            .IsRequired();

        builder.HasOne(x => x.Wallet)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Category)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Currency)
            .IsRequired();

        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Amount)
            .HasColumnType("decimal")
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(x => x.Note)
            .HasMaxLength(TransactionConstraints.MaximumNoteLength);

        builder.Property(x => x.Label)
            .HasMaxLength(TransactionConstraints.MaximumNoteLength);

        builder.ToTable("Transactions", t =>
        {
            t.HasCheckConstraint("Ck_Transactions_Amount",
                $"Amount >= {TransactionConstraints.MinimalTransactionAmount}");
        });
    }
}