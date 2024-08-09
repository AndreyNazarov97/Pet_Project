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
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.FullName).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Experience).IsRequired();
        builder.Property(x => x.PetsAdopted).IsRequired();
        builder.Property(x => x.PetsFoundHomeQuantity).IsRequired();
        builder.Property(x => x.PetsInTreatment).IsRequired();

        
        builder.Property(v => v.PhoneNumber)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<PhoneNumber>(v, JsonSerializerOptions.Default));
        builder.Property(v => v.Requisites)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<Requisite>>(v, JsonSerializerOptions.Default))
            .HasColumnType("jsonb");
        builder.Property(v => v.SocialNetworks)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<SocialNetwork>>(v, JsonSerializerOptions.Default))
            .HasColumnType("jsonb");
    }
}