using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.Dtos.Discussions;
using PetProject.Discussions.Domain.Entity;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Infrastructure.Configurations.Read;

public class MessageConfiguration : IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(v => v.Id)
            .HasName("id");
    }
}