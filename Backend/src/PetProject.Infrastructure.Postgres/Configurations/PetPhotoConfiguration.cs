using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{
    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Path).IsRequired();
        builder.Property(x => x.IsMain).IsRequired();
    }
}