using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.Dtos.Accounts;

namespace PetProject.Accounts.Infrastructure.Configurations.Read;

public class RoleDtoConfiguration : IEntityTypeConfiguration<RoleDto>
{
    public void Configure(EntityTypeBuilder<RoleDto> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(v => v.RoleId)
            .HasName("id");
        
        builder.Property(x => x.RoleId)
            .HasColumnName("id");
        
        builder.Property(v => v.Name)
            .HasColumnName("name");
    }
}