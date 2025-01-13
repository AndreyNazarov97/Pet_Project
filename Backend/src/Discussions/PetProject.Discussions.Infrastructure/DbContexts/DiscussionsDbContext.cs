using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Domain.Entity;
using PetProject.Discussions.Domain.ValueObjects;

namespace PetProject.Discussions.Infrastructure.DbContexts;

public class DiscussionsDbContext : DbContext
{
    private readonly string _connectionString;


    public DiscussionsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbSet<Discussion> Discussions { get; set; }
    public DbSet<Message> Messages { get; set; }

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
        modelBuilder.HasDefaultSchema("discussions");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscussionsDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }
    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
    
}