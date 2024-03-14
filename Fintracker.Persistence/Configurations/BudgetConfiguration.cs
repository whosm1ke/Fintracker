using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasColumnType("date");
        builder.Property(x => x.ModifiedAt).HasColumnType("date");
        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).HasMaxLength(50);

        builder.HasMany(x => x.Categories)
            .WithMany()
            .UsingEntity(j => j.ToTable("BudgetCategory"));


        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(BudgetConstraints.MaximumNameLength);

        builder.Property(x => x.Balance)
            .IsRequired()
            .HasColumnType("decimal")
            .HasPrecision(12, 2);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Budgets)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(x => x.Wallet)
            .WithMany(x => x.Budgets)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .HasColumnType("date")
            .IsRequired();
        builder.Property(x => x.EndDate)
            .HasColumnType("date")
            .IsRequired();

        builder.ToTable("Budgets", t =>
        {
            t.HasCheckConstraint("CK_Budgets_EndStartDate", "\"StartDate\" <= \"EndDate\"");
            t.HasCheckConstraint("CK_Budgets_Balance", $"\"Balance\" <= {BudgetConstraints.MaxBalance}");
        });
    }
}