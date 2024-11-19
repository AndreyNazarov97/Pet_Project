using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Id,
                value => PetId.Create(value));

        builder.ComplexProperty(p => p.PetName, pb =>
        {
            pb.IsRequired();
            pb.Property(pn => pn.Value)
                .HasColumnName("pet_name")
                .HasMaxLength(Constants.MIN_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.GeneralDescription, vb =>
        {
            vb.Property(d => d.Value)
                .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
                .HasColumnName("general_description")
                .IsRequired();
        });
        
        builder.ComplexProperty(p => p.HealthInformation, vb =>
        {
            vb.Property(d => d.Value)
                .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
                .HasColumnName("health_information")
                .IsRequired(false);
        });
        
        builder.ComplexProperty(p => p.AnimalType, at =>
        {
            at.IsRequired();
            at.Property(a => a.SpeciesId)
                .HasConversion(
                    id => id.Id,
                    value => SpeciesId.Create(value))
                .HasColumnName("species_id");
            at.Property(a => a.BreedId)
                .HasConversion(
                    id => id.Id,
                    value => BreedId.Create(value))
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(p => p.Address, pb =>
        {
            pb.Property(p => p.Country)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("country");
            pb.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("city");
            
            pb.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("street");

            pb.Property(p => p.House)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("house");
            
            pb.Property(p => p.Flat)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("flat");
        });

        builder.ComplexProperty(p => p.PhysicalAttributes, pb =>
        {
            pb.Property(p => p.Weight)
                .HasColumnName("weight")
                .IsRequired();

            pb.Property(p => p.Height)
                .HasColumnName("height")
                .IsRequired();
        });

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("phone_number")
                .IsRequired();
        });

        builder.Property(p => p.BirthDate).IsRequired();

        builder.Property(p => p.IsCastrated).IsRequired();

        builder.Property(p => p.IsVaccinated).IsRequired();

        builder.Property(p => p.DateCreated).IsRequired();

        builder.OwnsOne(p => p.RequisitesList, rb =>
        {
            rb.ToJson("requisites");

            rb.OwnsMany(v => v.Requisites, pbr =>
            {
                pbr.Property(r => r.Title)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH);

                pbr.Property(r => r.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(Constants.EXTRA_TEXT_LENGTH);
            });
        });
        
        builder.OwnsOne(p => p.PetPhotosList, plb =>
        {
            plb.ToJson("pet_photos");

            plb.OwnsMany(pl => pl.PetPhotos, ppb =>
            {
                ppb.OwnsOne(petPhoto => petPhoto.Path, filePathBuilder =>
                {
                    filePathBuilder.Property(p => p.Path)
                        .IsRequired()
                        .HasColumnName("path")
                        .HasMaxLength(Constants.MAX_TEXT_LENGTH);
                });

                ppb.Property(p => p.IsMain)
                    .IsRequired()
                    .HasColumnName("is_main");
            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}