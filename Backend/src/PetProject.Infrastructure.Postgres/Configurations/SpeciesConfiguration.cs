using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagment.AggregateRoot;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => SpeciesId.FromGuid(id));
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

        builder.HasMany(x => x.Breeds)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}