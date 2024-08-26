using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Domain.PetManagement.AggregateRoot;
using PetProject.Domain.PetManagement.Entities;
using PetProject.Domain.SpeciesManagment.AggregateRoot;

namespace PetProject.Infrastructure.Postgres;

public class PetProjectDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public PetProjectDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Species> Species { get; set; }

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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
    
}