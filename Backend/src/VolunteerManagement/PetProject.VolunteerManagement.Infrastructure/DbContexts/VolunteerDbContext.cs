using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.VolunteerManagement.Domain.Aggregate;

namespace PetProject.VolunteerManagement.Infrastructure.DbContexts;

public class VolunteerDbContext : DbContext
{
    private readonly string _connectionString;

    public VolunteerDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbSet<Volunteer> Volunteers { get; set; }

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
        modelBuilder.HasDefaultSchema("volunteers");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerDbContext).Assembly);
    }
    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
    
}