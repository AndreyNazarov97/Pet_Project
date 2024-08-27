using Microsoft.EntityFrameworkCore;
using PetProject.Infrastructure.Postgres;

namespace PetProject.API;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PetProjectDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}