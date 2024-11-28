using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Infrastructure;
using PetProject.SpeciesManagement.Infrastructure.DataSeed;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;

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


        var authDbContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        await authDbContext.Database.MigrateAsync();
    }

    public static async Task SeedDatabases(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var volunteerDbContext = scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
        await VolunteerDbContextSeeder.SeedAsync(volunteerDbContext);

        var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesDbContext>();
        await SpeciesDbContextSeeder.SeedAsync(speciesDbContext);
    }
}