using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasColumnType("date");
        builder.Property(x => x.ModifiedAt).HasColumnType("date");
        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(CurrencyConstraints.MaxNameLength);

        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasMaxLength(CurrencyConstraints.MaxSymbolLength);
    }
}