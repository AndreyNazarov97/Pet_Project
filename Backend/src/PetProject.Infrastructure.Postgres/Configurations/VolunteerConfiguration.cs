using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => VolunteerId.Create(id));
        
        builder
            .ToTable(t =>
            {
                t.HasCheckConstraint("ck_volunteer_experience", "\"experience\" >= 0");
                t.HasCheckConstraint("ck_volunteer_pets_adopted", "\"pets_adopted\" >= 0");
                t.HasCheckConstraint("ck_volunteer_pets_found_home_quantity", "\"pets_found_home_quantity\" >= 0");
                t.HasCheckConstraint("ck_volunteer_pets_in_treatment", "\"pets_in_treatment\" >= 0");
            });
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
        
        builder.Property(x => x.Experience)
            .IsRequired();
        
        builder.Property(x => x.PetsAdopted)
            .IsRequired();
        
        builder.Property(x => x.PetsFoundHomeQuantity)
            .IsRequired();
        
        builder.Property(x => x.PetsInTreatment)
            .IsRequired();
        
        builder.ComplexProperty(x => x.PhoneNumber, p =>
        {
            p.IsRequired();
            p.Property(x => x.Number)
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.PHONE_NUMBER_MAX_LENGTH);
        });
        
        builder.ComplexProperty(x => x.FullName, f =>
        {
            f.IsRequired();
            f.Property(f => f.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            f.Property(f => f.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            f.Property(f => f.Patronymic)
                .HasColumnName("patronymic")
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
        });
        
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