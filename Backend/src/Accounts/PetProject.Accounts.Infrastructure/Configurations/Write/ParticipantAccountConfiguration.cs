using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Infrastructure.Configurations.Write;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");
        
        builder
            .HasOne(pa => pa.User)
            .WithOne(u => u.ParticipantAccount)
            .HasForeignKey<ParticipantAccount>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}