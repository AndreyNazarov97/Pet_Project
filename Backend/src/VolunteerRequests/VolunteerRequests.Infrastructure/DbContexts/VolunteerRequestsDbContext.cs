using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Domain.Aggregate;

namespace VolunteerRequests.Infrastructure.DbContexts;

public class VolunteerRequestsDbContext : DbContext
{
    private readonly string _connectionString;


    public VolunteerRequestsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbSet<VolunteerRequest> VolunteerRequests { get; set; }
    
    public DbSet<UserRestriction> UserRestrictions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder  
            .UseNpgsql(_connectionString)
            .UseSnakeCaseNamingConvention()
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("volunteers_requests");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerRequestsDbContext).Assembly);
    }
    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
    
}