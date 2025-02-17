using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Accounts.Domain;
using PetProject.Core.Dtos;
using PetProject.Core.Extensions;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Infrastructure.Configurations.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder
            .HasMany(u => u.Roles)
            .WithMany();
        
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
        
        builder
            .Property(u => u.SocialNetworks)
            .HasValueObjectsJsonConversion(
                input => new SocialNetworkDto() { Title = input.Title , Url = input.Url},
                output => SocialNetwork.Create(output.Title, output.Url).Value)
            .HasColumnName("social_networks");
        
        builder
            .Property(u => u.Photos)
            .HasValueObjectsJsonConversion(
                input => new PhotoDto() { FileId = input.FileId, IsMain = input.IsMain},
                output => new Photo(output.FileId){IsMain = output.IsMain})
            .HasColumnName("photos");
        
    }
}