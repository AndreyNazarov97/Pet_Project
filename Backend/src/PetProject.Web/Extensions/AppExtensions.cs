using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Infrastructure.DataSeed;
using PetProject.Accounts.Infrastructure.DbContexts;
using PetProject.Discussions.Infrastructure.DbContexts;
using PetProject.SpeciesManagement.Infrastructure.DataSeed;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;
using VolunteerRequests.Infrastructure.DbContexts;

namespace PetProject.Web.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var volunteerDbContext = scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
        await volunteerDbContext.Database.MigrateAsync();

        var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesDbContext>();
        await speciesDbContext.Database.MigrateAsync();
        
        var accountsDbContext = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();
        await accountsDbContext.Database.MigrateAsync();
        
        var volunteerRequestsDbContext = scope.ServiceProvider.GetRequiredService<VolunteerRequestsDbContext>();
        await volunteerRequestsDbContext.Database.MigrateAsync();
        
        var discussionsDbContext = scope.ServiceProvider.GetRequiredService<DiscussionsDbContext>();
        await discussionsDbContext.Database.MigrateAsync();
    }

    public static async Task SeedDatabases(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var volunteersSeeder = scope.ServiceProvider.GetService<VolunteersSeeder>();
        if (volunteersSeeder is not null)
        {
            await volunteersSeeder.SeedAsync();
        }

        var speciesSeeder = scope.ServiceProvider.GetService<SpeciesSeeder>();
        if (speciesSeeder is not null)
        {
            await speciesSeeder.SeedAsync();
        }
        
        var accountSeeder = scope.ServiceProvider.GetService<AccountsSeeder>();
        if (accountSeeder is not null)
        {
            await accountSeeder.SeedAsync();
        }
            
    }
}