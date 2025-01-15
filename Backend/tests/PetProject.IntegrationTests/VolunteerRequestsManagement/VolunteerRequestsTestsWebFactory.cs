using System.Data.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using NSubstitute;
using PetProject.Accounts.Infrastructure.DataSeed;
using PetProject.Accounts.Infrastructure.DbContexts;
using PetProject.Core.Database;
using PetProject.Discussions.Contracts;
using PetProject.Discussions.Contracts.Requests;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Infrastructure.DbContexts;
using PetProject.SharedKernel.Shared;
using PetProject.SpeciesManagement.Infrastructure.DataSeed;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.Common;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;
using VolunteerRequests.Infrastructure.DbContexts;

namespace PetProject.IntegrationTests.VolunteerRequestsManagement;

public class VolunteerRequestsTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IDiscussionContract _discussionContract =
        Substitute.For<IDiscussionContract>();

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:alpine")
        .WithDatabase("pet_project")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner;
    private DbConnection _dbConnection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Изменяем строку подключения
            var builtConfig = config.Build();
            var newConnectionString = _dbContainer.GetConnectionString();

            // Здесь мы модифицируем конфигурацию
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:Postgres", newConnectionString }
            }!);
        });

        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    private void ConfigureDefaultServices(IServiceCollection services)
    {
        services.RemoveAll(typeof(IHostedService));
        services.RemoveAll(typeof(AccountsSeeder));
        services.RemoveAll(typeof(VolunteersSeeder));
        services.RemoveAll(typeof(SpeciesSeeder));
        

        services.RemoveAll(typeof(IDiscussionContract));

        services.AddScoped<IDiscussionContract>(_ => _discussionContract);
    }

    public void SetupCreate(Result<Discussion, ErrorList> result)
    {
        _discussionContract
            .Create(Arg.Any<CreateDiscussionRequest>(), Arg.Any<CancellationToken>())
            .Returns(result);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();

        var volunteerRequestsDbContext = scope.ServiceProvider.GetRequiredService<VolunteerRequestsDbContext>();
        await volunteerRequestsDbContext.Database.MigrateAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["volunteers", "accounts", "species", "volunteer_requests", "discussions"]
            }
        );
    }

    public async Task ResetDatabaseAsync()
    {
        await using var command = _dbConnection.CreateCommand();
        command.CommandText = @"
        DO $$ DECLARE
        r RECORD;
        BEGIN
            FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname in ('public')) LOOP
                EXECUTE 'TRUNCATE TABLE ' || quote_ident(r.tablename) || ' CASCADE';
            END LOOP;
        END $$;";
        await command.ExecuteNonQueryAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}