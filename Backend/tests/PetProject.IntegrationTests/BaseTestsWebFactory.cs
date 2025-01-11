using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using PetProject.Accounts.Infrastructure.DataSeed;
using PetProject.Accounts.Infrastructure.DbContexts;
using PetProject.Core.Database;
using PetProject.Discussions.Infrastructure.DbContexts;
using PetProject.SpeciesManagement.Infrastructure.DataSeed;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.Common;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;
using VolunteerRequests.Infrastructure.DbContexts;

namespace PetProject.IntegrationTests;

public class BaseTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
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

        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        services.RemoveAll(typeof(VolunteerDbContext));
        services.RemoveAll(typeof(AccountsDbContext));
        services.RemoveAll(typeof(SpeciesDbContext));
        services.RemoveAll(typeof(VolunteerRequestsDbContext));
        services.RemoveAll(typeof(DiscussionsDbContext));
        services.RemoveAll(typeof(IHostedService));
        services.RemoveAll(typeof(AccountsSeeder));
        services.RemoveAll(typeof(VolunteersSeeder));
        services.RemoveAll(typeof(SpeciesSeeder));
        services.RemoveAll(typeof(IPostgresConnectionFactory));
        
        services.AddScoped<VolunteerDbContext>(_ =>
            new VolunteerDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<AccountsDbContext>(_ =>
            new AccountsDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<SpeciesDbContext>(_ =>
            new SpeciesDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<VolunteerRequestsDbContext>(_ =>
            new VolunteerRequestsDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<DiscussionsDbContext>(_ =>
            new DiscussionsDbContext(_dbContainer.GetConnectionString()));
        services.AddSingleton<IPostgresConnectionFactory>(_ =>
            new PostgresConnectionFactory(_dbContainer.GetConnectionString()));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        
        var volunteerDbContext = scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
        await volunteerDbContext.Database.MigrateAsync();
        
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
        await _respawner.ResetAsync(_dbConnection); 
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}