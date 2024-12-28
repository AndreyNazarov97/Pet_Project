using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Domain.Aggregate;

namespace VolunteerRequests.Infrastructure.DbContexts;

public class VolunteerRequestsDbContext : DbContext
{
    private readonly IConfiguration _configuration;


    public VolunteerRequestsDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<VolunteerRequest> VolunteerRequests { get; set; }

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
        modelBuilder.HasDefaultSchema("volunteers_requests");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerRequestsDbContext).Assembly);
    }
    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
    
}