using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.Dtos.Discussions;
using PetProject.Discussions.Domain.Entity;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Infrastructure.Configurations.Read;

public class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(v => v.Id)
            .HasName("id");
        
    }
}