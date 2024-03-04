using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

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

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Fintracker.Persistence/InitialData/currencies.json");
        var currencies = JsonConvert.DeserializeObject<List<Currency>>(File.ReadAllText(path));

        if (currencies != null) builder.HasData(currencies);
    }
}