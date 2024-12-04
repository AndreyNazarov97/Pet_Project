﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Domain;

namespace PetProject.Accounts.Infrastructure;

public class AccountsDbContext
    : IdentityDbContext<User, Role, long>
{
    private readonly IConfiguration _configuration;
    
    
    public DbSet<AdminAccount> AdminAccounts { get; set; }
    public DbSet<VolunteerAccount> VolunteerAccounts { get; set; }
    public DbSet<ParticipantAccount> ParticipantAccounts { get; set; }
    public override DbSet<Role> Roles { get; set; } 
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    
    public AccountsDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("Postgres");
        optionsBuilder  
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Role>()
            .ToTable("roles");
        
        modelBuilder.Entity<Permission>()
            .ToTable("permissions");
        
        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();
        
        modelBuilder.Entity<IdentityUserClaim<long>>()
            .ToTable("user_claims");

        modelBuilder.Entity<IdentityUserToken<long>>()
            .ToTable("user_tokens");

        modelBuilder.Entity<IdentityUserLogin<long>>()
            .ToTable("user_logins");

        modelBuilder.Entity<IdentityRoleClaim<long>>()
            .ToTable("role_claims");

        modelBuilder.Entity<IdentityUserRole<long>>()
            .ToTable("user_roles");
        
        modelBuilder.HasDefaultSchema("accounts");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountsDbContext).Assembly);
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}