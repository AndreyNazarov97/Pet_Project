﻿using Microsoft.EntityFrameworkCore;
using PetProject.Infrastructure.Authorization;
using PetProject.Infrastructure.Postgres;

namespace PetProject.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var petProjectDbContext = scope.ServiceProvider.GetRequiredService<PetProjectDbContext>();
        await petProjectDbContext.Database.MigrateAsync();
        var authDbContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        await authDbContext.Database.MigrateAsync();
    }
}