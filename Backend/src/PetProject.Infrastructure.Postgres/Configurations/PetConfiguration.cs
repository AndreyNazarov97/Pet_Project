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
        builder.ToTable("pets");
        
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
        
        builder.ComplexProperty(p => p.Address);
        builder.ComplexProperty(v => v.OwnerPhoneNumber);

        builder.OwnsMany(x => x.Requisites, r =>
        {
            r.ToJson();
        });
            
        builder
            .HasMany(x => x.Photos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}