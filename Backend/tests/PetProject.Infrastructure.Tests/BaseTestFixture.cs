using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetProject.Infrastructure.Postgres;
using PetProject.Infrastructure.Postgres.Abstractions;
using Testcontainers.PostgreSql;

namespace PetProject.Infrastructure.Tests;

public class BaseTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder().Build();
    private IConfiguration _configuration;

    public PetProjectDbContext GetDbContext() => new(_configuration);

    public IPostgresConnectionFactory GetConnectionFactory() =>
        new TestPostgresConnectionFactory(_postgresSqlContainer);

    public virtual async Task InitializeAsync()
    {
        await _postgresSqlContainer.StartAsync();

        var connectionString = _postgresSqlContainer.GetConnectionString();
        if (string.IsNullOrEmpty(connectionString) || !connectionString.Contains("Host"))
        {
            throw new InvalidOperationException("Host can't be null in the connection string.");
        }

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:Postgres", _postgresSqlContainer.GetConnectionString() }
            }!)
            .Build();

        var dbContext = GetDbContext();
        await dbContext.Database.MigrateAsync();
    }

    public virtual async Task DisposeAsync()
    {
        await _postgresSqlContainer.StopAsync();
        await _postgresSqlContainer.DisposeAsync();
    }

    public async Task ClearDatabaseAsync(params string[] tables)
    {
        await using var dbContext = GetDbContext();
        foreach (var table in tables)
        {
            await dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {table} CASCADE;");
        }
    }
}