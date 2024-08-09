using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.FullName).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Experience).IsRequired();
        builder.Property(x => x.PetsAdopted).IsRequired();
        builder.Property(x => x.PetsFoundHomeQuantity).IsRequired();
        builder.Property(x => x.PetsInTreatment).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();
        
        builder
            .HasMany(x => x.Requisites)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasMany(x => x.SocialNetworks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}