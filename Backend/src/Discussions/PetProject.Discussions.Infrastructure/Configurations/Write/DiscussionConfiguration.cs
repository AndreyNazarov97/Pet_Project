using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Domain.Entity;
using PetProject.Discussions.Domain.ValueObjects;
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
        
        builder.ComplexProperty(x => x.Members, b =>
            {
                b.Property(m => m.FirstMemberId)
                    .HasColumnName("first_member_id")
                    .IsRequired();
                
                b.Property(m => m.SecondMemberId)
                    .HasColumnName("second_member_id")
                    .IsRequired();
            }
        );
        
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasMany(v => v.Messages)
            .WithOne()
            .HasForeignKey("discussion_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(v => v.Messages).AutoInclude();
    }
}