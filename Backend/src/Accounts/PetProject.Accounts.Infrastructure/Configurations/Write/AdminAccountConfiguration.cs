using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Infrastructure.Configurations.Write;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");
        
        builder
            .HasOne(aa => aa.User)
            .WithOne(u => u.AdminAccount)
            .HasForeignKey<AdminAccount>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}