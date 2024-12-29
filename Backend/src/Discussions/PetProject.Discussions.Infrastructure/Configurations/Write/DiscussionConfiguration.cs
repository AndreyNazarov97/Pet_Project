using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Domain.Entity;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Infrastructure.Configurations.Write;

public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(x => x.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Id,
                value => DiscussionId.Create(value));
        
        builder.HasOne(x => x.Members)
            .WithOne()
            .HasForeignKey<Members>("discussion_id") 
            .OnDelete(DeleteBehavior.Cascade);
        
        
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasMany(v => v.Messages)
            .WithOne()
            .HasForeignKey("discussion_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(v => v.Messages).AutoInclude();
        
        builder.Navigation(v => v.Members).AutoInclude();
    }
}