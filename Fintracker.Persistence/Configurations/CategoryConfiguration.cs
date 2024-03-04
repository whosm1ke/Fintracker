using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(CategoryConstraints.MaximumNameLength)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(x => x.Image)
            .IsRequired()
            .HasMaxLength(CategoryConstraints.MaximumImageLength);
        
        builder.Property(x => x.IconColour)
            .IsRequired()
            .HasMaxLength(CategoryConstraints.MaximumIconColourLength);
    }
}