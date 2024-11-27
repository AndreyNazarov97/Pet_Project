using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SpeciesManagement.Domain.Entities;

namespace PetProject.SpeciesManagement.Infrastructure.Configurations;

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
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}