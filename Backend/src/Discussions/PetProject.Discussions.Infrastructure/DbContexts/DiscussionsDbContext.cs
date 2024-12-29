using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Domain.Entity;

namespace PetProject.Discussions.Infrastructure.DbContexts;

public class DiscussionsDbContext : DbContext
{
    private readonly IConfiguration _configuration;


    public DiscussionsDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<Discussion> Discussions { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Members> Members { get; set; }

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
        modelBuilder.HasDefaultSchema("discussions");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscussionsDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }
    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
    
}