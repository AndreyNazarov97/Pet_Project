using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.PetManagement.Entities;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{
    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.ToTable("pet_photos");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => PetPhotoId.FromGuid(id));
        
        builder.Property(x => x.Path)
            .IsRequired()
            .HasMaxLength(Constants.MAX_PATH_LENGTH);
        
        builder.Property(x => x.Id)
            .IsRequired();
        
        builder.Property(x => x.IsMain)
            .IsRequired();
    }
}