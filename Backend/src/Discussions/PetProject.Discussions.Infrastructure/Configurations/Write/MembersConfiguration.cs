using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Discussions.Domain.Entity;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Infrastructure.Configurations.Write;

public class MembersConfiguration : IEntityTypeConfiguration<Members>
{
    public void Configure(EntityTypeBuilder<Members> builder)
    {
        builder.ToTable("members");

        builder.HasKey(x => x.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Id,
                value => MemberId.Create(value));

        builder.Property(x => x.FirstMemberId)
            .IsRequired();

        builder.Property(x => x.SecondMemberId)
            .IsRequired();
    }
}