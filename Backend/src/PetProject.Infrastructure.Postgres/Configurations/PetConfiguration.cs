using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.PetManagement.Entities;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

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
        
        builder.ComplexProperty(x => x.Name, p =>
        {
            p.IsRequired();
            p.Property(x => x.Value)
                .HasColumnName("name")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                
        });

        builder.ComplexProperty(p => p.Description, pb =>
        {
            pb.IsRequired();
            pb.Property(x => x.Value)
                .HasColumnName("description")
                .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);

        });

        builder.ComplexProperty(p => p.BreedName, pb =>
        {
            pb.IsRequired();
            pb.Property(x => x.Value)
                .HasColumnName("breed_name")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

        });

        builder.ComplexProperty(p => p.Color, pb =>
        {
            pb.IsRequired();
            pb.Property(x => x.Value)
                .HasColumnName("color")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

        });

        builder.ComplexProperty(p => p.HealthInfo, pb =>
        {
            pb.IsRequired();
            pb.Property(x => x.Value)
                .HasColumnName("health_info")
                .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);

        });

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

        builder.Navigation(p => p.Photos).AutoInclude();
    }
}