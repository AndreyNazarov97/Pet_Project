using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagment.Entities;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => BreedId.FromGuid(id));

        builder.ComplexProperty(x => x.Name, pb =>
        {
            pb.IsRequired();
            pb.Property(x => x.Value)
                .HasColumnName("name")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

        });
    }
}