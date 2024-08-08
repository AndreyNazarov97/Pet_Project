﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace PetProject.Infrastructure.Postgres;

public class PetProjectDbContext : DbContext
{
    public PetProjectDbContext(DbContextOptions<PetProjectDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}