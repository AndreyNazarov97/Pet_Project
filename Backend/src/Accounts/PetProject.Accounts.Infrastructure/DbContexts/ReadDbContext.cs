using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Application.Interfaces;
using PetProject.Core.Dtos.Accounts;

namespace PetProject.Accounts.Infrastructure.DbContexts;

public class ReadDbContext : DbContext, IReadDbContext
{
    private readonly string _connectionString;

    public ReadDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IQueryable<UserDto> Users => Set<UserDto>();
    public IQueryable<AdminAccountDto> AdminAccounts => Set<AdminAccountDto>();
    public IQueryable<ParticipantAccountDto> ParticipantAccounts => Set<ParticipantAccountDto>();
    public IQueryable<VolunteerAccountDto> VolunteerAccounts => Set<VolunteerAccountDto>();
    public IQueryable<RoleDto> Roles => Set<RoleDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("accounts");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_connectionString)
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