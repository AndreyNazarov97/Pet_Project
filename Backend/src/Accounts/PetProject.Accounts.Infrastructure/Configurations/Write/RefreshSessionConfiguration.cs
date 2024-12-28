using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Infrastructure.Configurations.Write;

public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.ToTable("refresh_sessions");

        builder
            .HasOne(rs => rs.User)
            .WithMany()
            .HasForeignKey(rs => rs.UserId);
        
        builder.Navigation(v => v.User).AutoInclude();
    }
}