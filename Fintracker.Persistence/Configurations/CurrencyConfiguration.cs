using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(CurrencyConstraints.MaxNameLength);
        
        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasMaxLength(CurrencyConstraints.MaxSymbolLength);
    }
}