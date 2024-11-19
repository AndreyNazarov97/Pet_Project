using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Id,
                result => BreedId.Create(result)
            );

        builder.ComplexProperty(b => b.BreedName, bb =>
        {
            bb.IsRequired();
            bb.Property(bn => bn.Value)
                .HasColumnName("breed_name")
                .HasMaxLength(Constants.MIN_TEXT_LENGTH);
        }); 
    }
}