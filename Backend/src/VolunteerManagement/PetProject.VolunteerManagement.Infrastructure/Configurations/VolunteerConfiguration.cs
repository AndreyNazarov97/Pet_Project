using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.Dtos;
using PetProject.Core.Extensions;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.Aggregate;

namespace PetProject.VolunteerManagement.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Id,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName, vb =>
        {
            vb.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("name");

            vb.Property(v => v.Surname)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("surname");

            vb.Property(v => v.Patronymic)
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("patronymic");
        });


        builder.ComplexProperty(v => v.GeneralDescription, vb =>
        {
            vb.Property(d => d.Value)
                .HasColumnName("general_description")
                .HasMaxLength(Constants.MAX_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.Experience, vb =>
        {
            vb.Property(v => v.Years)
                .HasColumnName("age_experience")
                .IsRequired();
        });

        builder.ComplexProperty(v => v.PhoneNumber, vb =>
        {
            vb.Property(p => p.Value)
                .HasColumnName("phone_number")
                .IsRequired();
        });

        builder.Property(v => v.SocialLinks)
            .HasValueObjectsJsonConversion(
                input => new SocialLinkDto { Title = input.Title, Url = input.Url },
                output => SocialNetwork.Create(output.Title, output.Url).Value)
            .HasColumnName("social_links");

        builder.Property(v => v.Requisites)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto { Title = input.Title, Description = input.Description },
                output => Requisite.Create(output.Title, output.Description).Value)
            .HasColumnName("requisites");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.Pets).AutoInclude();
    }
}