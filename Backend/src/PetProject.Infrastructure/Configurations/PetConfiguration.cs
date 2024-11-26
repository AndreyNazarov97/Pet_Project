using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Application.Dto;
using PetProject.Domain.VolunteerManagement.ValueObjects;
using PetProject.Infrastructure.Postgres.Extensions;

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
            at.Property(a => a.SpeciesName)
                .HasConversion(
                    s => s.Value,
                    value => SpeciesName.Create(value).Value)
                .HasColumnName("species_name");
            at.Property(a => a.BreedName)
                .HasConversion(
                    b => b.Value,
                    value => BreedName.Create(value).Value)
                .HasColumnName("breed_name");
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

        builder.ComplexProperty(p => p.Position, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("position")
                .IsRequired();
        });

        builder.Property(p => p.BirthDate).IsRequired();

        builder.Property(p => p.IsCastrated).IsRequired();

        builder.Property(p => p.IsVaccinated).IsRequired();

        builder.Property(p => p.DateCreated).IsRequired();

        builder.Property(p => p.Requisites)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto(input.Title, input.Description) ,
                output => Requisite.Create(output.Title, output.Description).Value)
            .HasColumnName("requisites");

        builder.Property(v => v.PetPhotoList)
            .HasValueObjectsJsonConversion(
                input => new PetPhotoDto() { Path = input.Path.Path , IsMain = input.IsMain},
                output => new PetPhoto(FilePath.Create(output.Path).Value){IsMain = output.IsMain})
            .HasColumnName("pet_photos");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}