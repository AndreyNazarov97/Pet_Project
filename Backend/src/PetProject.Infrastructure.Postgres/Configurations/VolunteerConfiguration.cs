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

        builder.OwnsOne(v => v.Details, d =>
        {
            d.ToJson();
            d.OwnsMany(vd => vd.SocialNetworks, sn =>
            {
                sn.Property(s => s.Title)
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                sn.Property(s => s.Link)
                    .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
            });

            d.OwnsMany(vd => vd.Requisites, r =>
            {
                r.Property(r => r.Title)
                    .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                r.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
            });
        });

        builder
            .HasMany(x => x.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.Pets).AutoInclude();
    }
}