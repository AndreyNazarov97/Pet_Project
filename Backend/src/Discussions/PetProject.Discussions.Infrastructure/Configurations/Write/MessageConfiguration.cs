using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Discussions.Domain.Entity;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Infrastructure.Configurations.Write;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(x => x.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Id,
                value => MessageId.Create(value));

        builder.ComplexProperty(x => x.Text, b =>
            {
                b.Property(t => t.Value)
                    .HasColumnName("text")
                    .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
                    .IsRequired();
            }
        );
    }
}