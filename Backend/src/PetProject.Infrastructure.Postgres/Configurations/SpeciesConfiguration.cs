﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.PetManagement.Entities;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Infrastructure.Postgres.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => SpeciesId.NewSpeciesId());
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);

        builder.HasMany(x => x.Breeds)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}