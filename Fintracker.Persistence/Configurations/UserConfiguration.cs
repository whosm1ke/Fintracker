using Fintracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintracker.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(x => x.Budgets)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.OwnsOne(x => x.UserDetails)
            .Property(x => x.DateOfBirth)
            .HasColumnType("date");
        
    }
}