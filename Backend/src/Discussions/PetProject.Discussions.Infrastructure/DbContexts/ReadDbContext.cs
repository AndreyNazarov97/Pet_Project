using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Core.Dtos.Discussions;
using PetProject.Discussions.Application.Interfaces;

namespace PetProject.Discussions.Infrastructure.DbContexts;

public class ReadDbContext : DbContext, IReadDbContext
{
    private readonly IConfiguration _configuration;

    public ReadDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IQueryable<DiscussionDto> Discussions => Set<DiscussionDto>();
    public IQueryable<MessageDto> Messages => Set<MessageDto>();
    public IQueryable<MembersDto> Members => Set<MembersDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("discussions");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("Postgres");
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}