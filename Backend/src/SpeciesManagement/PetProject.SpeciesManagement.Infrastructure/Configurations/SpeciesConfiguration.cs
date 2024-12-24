using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SpeciesManagement.Domain.Aggregate;

namespace PetProject.SpeciesManagement.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                speciesId => speciesId.Id,
                result => SpeciesId.Create(result));

        builder.ComplexProperty(s => s.Name, sb =>
        {
            sb.IsRequired();
            sb.Property(sn => sn.Value)
                .HasColumnName("species_name")
                .HasMaxLength(Constants.MIN_TEXT_LENGTH);
        });

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(s => s.Breeds).AutoInclude();
    }
}