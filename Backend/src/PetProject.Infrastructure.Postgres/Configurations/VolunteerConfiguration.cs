using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Experience).IsRequired();
        builder.Property(x => x.PetsAdopted).IsRequired();
        builder.Property(x => x.PetsFoundHomeQuantity).IsRequired();
        builder.Property(x => x.PetsInTreatment).IsRequired();
        
        builder.ComplexProperty(x => x.PhoneNumber);
        builder.ComplexProperty(x => x.FullName);
        
        builder.OwnsMany(v => v.Requisites, r =>
        {
            r.ToJson();
        });
        builder.OwnsMany(v => v.SocialNetworks, r =>
        {
            r.ToJson();
        });
    }
}