using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class RequisiteConfiguration : IEntityTypeConfiguration<Requisite>
{
    public void Configure(EntityTypeBuilder<Requisite> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Description).IsRequired();
    }
}