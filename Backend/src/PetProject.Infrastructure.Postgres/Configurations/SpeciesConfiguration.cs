using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired();

        builder.HasMany(x => x.Breeds)
            .WithOne()
            .HasForeignKey(b => b.SpeciesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Pet>()
            .WithOne()
            .HasForeignKey(p => p.SpeciesId);
    }
}