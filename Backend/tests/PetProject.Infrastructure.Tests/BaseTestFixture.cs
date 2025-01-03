﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetProject.Core.Database;
using PetProject.Discussions.Infrastructure.DbContexts;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;
using VolunteerRequests.Infrastructure.DbContexts;

namespace PetProject.Infrastructure.Tests;

public class BaseTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder().Build();
    private IConfiguration _configuration;

    public VolunteerDbContext GetVolunteerDbContext() => new(_configuration);
    public SpeciesDbContext GetSpeciesDbContext() => new(_configuration);
    
    public VolunteerRequestsDbContext GetVolunteerRequestsDbContext() => new(_configuration);
    public DiscussionsDbContext GetDiscussionsDbContext() => new(_configuration);

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
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:Postgres", _postgresSqlContainer.GetConnectionString() }
            }!)
            .Build();

        var volunteerDbContext = GetVolunteerDbContext();
        await volunteerDbContext.Database.MigrateAsync();
        
        var speciesDbContext = GetSpeciesDbContext();
        await speciesDbContext.Database.MigrateAsync();
        
        var volunteerRequestsDbContext = GetVolunteerRequestsDbContext();
        await volunteerRequestsDbContext.Database.MigrateAsync();
        
        var discussionsDbContext = GetDiscussionsDbContext();
        await discussionsDbContext.Database.MigrateAsync();
    }

    public virtual async Task DisposeAsync()
    {
        await _postgresSqlContainer.StopAsync();
        await _postgresSqlContainer.DisposeAsync();
    }

    public async Task ClearDatabaseAsync(string schema ,params string[] tables)
    {
        await using var dbContext = GetVolunteerDbContext();
        foreach (var table in tables)
        {
            await dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {schema}.{table} CASCADE;");
        }
    }
}