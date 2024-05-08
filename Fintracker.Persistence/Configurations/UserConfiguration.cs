using Fintracker.Application;
using Fintracker.Application.BusinessRuleConstants;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Fintracker.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasColumnType("date");
        builder.Property(x => x.ModifiedAt).HasColumnType("date");
        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).HasMaxLength(50);

        builder.HasMany(x => x.OwnedBudgets)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId);

        builder.HasMany(x => x.Categories)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CurrencyId)
            .HasDefaultValue(new Guid("c6746fe4-eb4c-1746-0c5e-88d8748deebc"));

        builder.HasOne(x => x.UserDetails)
            .WithOne()
            .HasForeignKey<UserDetails>(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserDetailsConfiguration : IEntityTypeConfiguration<UserDetails>
{
   

    public void Configure(EntityTypeBuilder<UserDetails> builder)
    {
        //       AddedDefaultAvatarToUser
        builder.HasKey(u => u.Id);

        builder.ToTable("UserDetails",
            ba => { ba.HasCheckConstraint("CK_UserDetails_Birthday", "\"Birthday\" >= '1915-01-01'"); });

        builder.Property(x => x.DateOfBirth)
            .HasColumnType("date")
            .HasColumnName("Birthday");

        builder.Property(x => x.Avatar)
            .HasMaxLength(UserDetailsConstraints.MaxAvatarLength);

        builder.Property(x => x.Sex)
            .HasMaxLength(UserDetailsConstraints.MaxSexLength);

        builder.Property(x => x.Language)
            .HasConversion(
                v => v.ToString(),
                v => (Language)Enum.Parse(typeof(Language), v));
    }
}