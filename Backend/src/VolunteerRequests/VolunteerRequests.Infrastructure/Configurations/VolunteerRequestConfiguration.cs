using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.Dtos;
using PetProject.Core.Extensions;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using VolunteerRequests.Domain.Aggregate;

namespace VolunteerRequests.Infrastructure.Configurations;

public class VolunteerRequestConfiguration : IEntityTypeConfiguration<VolunteerRequest>
{
    public void Configure(EntityTypeBuilder<VolunteerRequest> builder)
    {
        builder.ToTable("volunteer_requests");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Id,
                value => VolunteerRequestId.Create(value));

        builder.OwnsOne(vr => vr.VolunteerInfo, vib =>
        {
            vib.OwnsOne(vi => vi.FullName, fb =>
            {
                fb.Property(v => v.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("name");

                fb.Property(v => v.Surname)
                    .IsRequired()
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("surname");

                fb.Property(v => v.Patronymic)
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("patronymic");
            });

            vib.OwnsOne(vi => vi.PhoneNumber, pb =>
            {
                pb.Property(p => p.Value)
                    .HasColumnName("phone_number")
                    .IsRequired();
            });
            
            vib.OwnsOne(vi => vi.GeneralDescription, gb =>
            {
                gb.Property(d => d.Value)
                    .HasColumnName("general_description")
                    .HasMaxLength(Constants.MAX_TEXT_LENGTH);
            });
            
            vib.OwnsOne(vi => vi.WorkExperience, wb =>
            {
                wb.Property(v => v.Years)
                    .HasColumnName("age_experience")
                    .IsRequired();
            });
            
            vib.Property(v => v.SocialNetworks)
                .HasValueObjectsJsonConversion(
                    s => new SocialNetworkDto {Title = s.Title, Url = s.Url},
                    dto => SocialNetwork.Create(dto.Title, dto.Url).Value)
                .HasColumnName("social_networks");
        });
        
        builder.ComplexProperty(v => v.RejectionComment, r =>
        {
            r.Property(x => x.Value)
                .HasColumnName("rejection_comment")
                .IsRequired(false);
        });
        
        builder.Property(p => p.RequestStatus)
            .HasConversion<string>()
            .IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}