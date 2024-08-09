using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired();
        builder.Property(p => p.Type).IsRequired();
        builder.Property(p => p.Description).IsRequired();
        builder.Property(p => p.Breed).IsRequired();
        builder.Property(p => p.Color).IsRequired();
        builder.Property(p => p.HealthInfo).IsRequired();
        builder.Property(p => p.Weight).IsRequired();
        builder.Property(p => p.Height).IsRequired(); 
        builder.Property(p => p.IsCastrated).IsRequired(); 
        builder.Property(p => p.BirthDate).IsRequired(); 
        builder.Property(p => p.IsVaccinated).IsRequired(); 
        builder.Property(p => p.HelpStatus).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        
        
        builder.Property(v => v.Address)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<Adress>(v, JsonSerializerOptions.Default))
            .HasColumnType("jsonb");
        builder.Property(v => v.OwnerPhoneNumber)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<PhoneNumber>(v, JsonSerializerOptions.Default));
        builder.Property(v => v.Requisites)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<Requisite>>(v, JsonSerializerOptions.Default))
            .HasColumnType("jsonb");

        builder
            .HasMany(x => x.Photos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}