using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.SpeciesManagement.Domain.Aggregate;

namespace PetProject.SpeciesManagement.Infrastructure.DbContexts;

public class SpeciesDbContext : DbContext
{
    private readonly string _connectionString;


    public SpeciesDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbSet<Species> Species { get; set; }

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
        modelBuilder.HasDefaultSchema("species");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesDbContext).Assembly);
    }
    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
    
}