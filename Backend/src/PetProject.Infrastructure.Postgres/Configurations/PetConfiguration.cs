using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_pet_weight", "\"weight\" > 0");
            t.HasCheckConstraint("ck_pet_height", "\"height\" > 0");
        });
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => PetId.NewPetId());
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
        
        builder.Property(p => p.BreedName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.Color)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        
        builder.Property(p => p.HealthInfo)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
        
        builder.Property(p => p.Weight)
            .IsRequired();
        
        builder.Property(p => p.Height)
            .IsRequired(); 
        
        builder.Property(p => p.IsCastrated)
            .IsRequired(); 
        
        builder.Property(p => p.BirthDate)
            .IsRequired(); 
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired(); 
        
        builder.Property(p => p.HelpStatus)
            .IsRequired();
        
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.ComplexProperty(p => p.AnimalType, at =>
        {
            at.IsRequired();
            at.Property(a => a.SpeciesId)
                .HasConversion(
                    id => id.Id,
                    id => SpeciesId.NewSpeciesId())
                .HasColumnName("species_id");
            at.Property(a => a.BreedId)
                .HasConversion(
                    id => id.Id,
                    id => BreedId.NewBreedId())
                .HasColumnName("breed_id");
        });
        builder.ComplexProperty(p => p.Address, a =>
        {
            a.IsRequired();
            a.Property(a => a.Country)
                .HasColumnName("country")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            a.Property(a => a.City)
                .HasColumnName("city")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            a.Property(a => a.Street)
                .HasColumnName("street")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            a.Property(a => a.House)
                .HasColumnName("house")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            a.Property(a => a.Flat)
                .HasColumnName("flat")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        });
        builder.ComplexProperty(v => v.OwnerPhoneNumber, p =>
        {
            p.IsRequired();
            p.Property(p => p.Number)
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.PHONE_NUMBER_MAX_LENGTH);
        });

        builder.OwnsOne(v => v.Details, d =>
        {
            d.ToJson();
          
            d.OwnsMany(vd => vd.Requisites, r =>
            {
                r.Property(r => r.Title)
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                r.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
            });
        });
            
        builder
            .HasMany(x => x.Photos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}