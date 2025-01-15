using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Domain.Aggregate;

namespace VolunteerRequests.Infrastructure.Configurations;

public class UserRestrictionConfiguration : IEntityTypeConfiguration<UserRestriction>
{
    public void Configure(EntityTypeBuilder<UserRestriction> builder)
    {
        builder.ToTable("user_restrictions");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Id,
                value => UserRestrictionId.Create(value));

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.IsDeleted);
        builder.Ignore(x => x.DeletionDate);
    }
}