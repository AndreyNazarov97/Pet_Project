using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.PetManagement.AggregateRoot;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

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
                id => VolunteerId.FromGuid(id));
        
        builder.ComplexProperty(x => x.Description, p =>
        {
            p.IsRequired();
            p.Property(x => x.Value)
                .HasColumnName("description")
                .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
        });
        
        builder.ComplexProperty(x => x.PhoneNumber, p =>
        {
            p.IsRequired();
            p.Property(x => x.Number)
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.PHONE_NUMBER_MAX_LENGTH);
        });

        builder.ComplexProperty(x => x.Experience, p =>
        {
            p.IsRequired();
            p.Property(x => x.Value).HasColumnName("experience");
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

        builder.OwnsOne(v => v.Requisites, rb =>
        {
            rb.ToJson("requisites");

            rb.OwnsMany(r => r.Requisites, requisitesBuilder =>
            {
                requisitesBuilder.Property(r => r.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                requisitesBuilder.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
            });
        });

        builder.OwnsOne(v => v.SocialNetworks, sb =>
        {
            sb.ToJson("social_networks");

            sb.OwnsMany(s => s.SocialNetworks, socialNetworksBuilder =>
            {
                socialNetworksBuilder.Property(s => s.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                socialNetworksBuilder.Property(s => s.Link)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
            });
        });

        builder
            .HasMany(x => x.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        //builder.Navigation(v => v.Pets).AutoInclude();
    }
}