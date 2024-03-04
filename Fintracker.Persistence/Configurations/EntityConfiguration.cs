using Fintracker.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class EntityConfiguration : IEntityTypeConfiguration<IEntity<Guid>>
{
    public void Configure(EntityTypeBuilder<IEntity<Guid>> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasColumnType("date");
        builder.Property(x => x.ModifiedAt).HasColumnType("date").IsConcurrencyToken();
        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).HasMaxLength(50);
    }
}