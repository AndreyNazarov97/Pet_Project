﻿using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Infrastructure;
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

        var volunteerDbContext = scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
        await VolunteerDbContextSeeder.SeedAsync(volunteerDbContext);

        var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesDbContext>();
        await SpeciesDbContextSeeder.SeedAsync(speciesDbContext);
        
        var accountSeeder = scope.ServiceProvider.GetRequiredService<AccountsSeeder>();
        await accountSeeder.SeedAsync();
    }
}