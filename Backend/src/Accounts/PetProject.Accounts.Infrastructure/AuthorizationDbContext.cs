using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PetProject.Accounts.Domain;
using PetProject.SharedKernel.Shared.ValueObjects;
using Constants = PetProject.SharedKernel.Constants.Constants;

namespace PetProject.Accounts.Infrastructure;

public class AuthorizationDbContext
    : IdentityDbContext<User, Role, long>
{
    private readonly IConfiguration _configuration;
    
    public AuthorizationDbContext(IConfiguration configuration)
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
        
        modelBuilder.Entity<User>()
            .ToTable("users");
        
        modelBuilder.Entity<User>()
            .Property(u => u.SocialNetworks)
            .HasConversion(
                v => JsonSerializer.Serialize(v,  JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<SocialNetwork>>(json, JsonSerializerOptions.Default)!,
                new ValueComparer<List<SocialNetwork>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList())) 
            ;

        modelBuilder.Entity<Role>()
            .ToTable("roles");
        
        modelBuilder.Entity<Permission>()
            .ToTable("permissions");
        
        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();
        
        modelBuilder.Entity<Permission>()
            .Property(p => p.Description)
            .HasMaxLength(Constants.MIDDLE_TEXT_LENGTH);
        
        modelBuilder.Entity<RolePermission>()
            .ToTable("role_permissions");
        
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => rp.Id);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);
        
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);
        
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
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}