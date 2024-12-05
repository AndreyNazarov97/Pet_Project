using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Accounts.Domain;
using PetProject.Core.Dtos;
using PetProject.Core.Extensions;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");
        
        builder
            .HasOne(va => va.User)
            .WithOne(u => u.VolunteerAccount)
            .HasForeignKey<VolunteerAccount>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ComplexProperty(v => v.Experience, vb =>
        {
            vb.Property(v => v.Years)
                .HasColumnName("experience")
                .IsRequired();
        });
        
        builder.Property(p => p.Requisites)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto { Title = input.Title, Description = input.Description },
                output => Requisite.Create(output.Title, output.Description).Value)
            .HasColumnName("requisites");
        
        builder.Property(p => p.Certificates)
            .HasConversion(
                v => JsonSerializer.Serialize(v,  JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<Certificate>>(json, JsonSerializerOptions.Default)!,
                new ValueComparer<List<Certificate>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .HasColumnName("certificates");
    }
}