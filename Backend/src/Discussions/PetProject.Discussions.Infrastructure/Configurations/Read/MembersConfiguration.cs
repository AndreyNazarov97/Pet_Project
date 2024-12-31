using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.Dtos.Discussions;
using PetProject.Discussions.Domain.Entity;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Infrastructure.Configurations.Read;

public class MembersConfiguration : IEntityTypeConfiguration<MembersDto>
{
    public void Configure(EntityTypeBuilder<MembersDto> builder)
    {
        builder.ToTable("members");

        builder.HasKey(v => v.Id)
            .HasName("id");
    }
}