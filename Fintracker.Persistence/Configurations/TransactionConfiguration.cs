using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasColumnType("date");
        builder.Property(x => x.ModifiedAt).HasColumnType("date");
        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).HasMaxLength(50);

        builder.Property(x => x.Date).HasColumnType("date");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Wallet)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

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
                $"\"Amount\" >= {TransactionConstraints.MinimalTransactionAmount} and \"Amount\" <" +
                $"= {TransactionConstraints.MaximumTransactionAmount}");
        });
    }
}