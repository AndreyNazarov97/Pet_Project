using Microsoft.Extensions.DependencyInjection;

namespace PetProject.VolunteerManagement.Infrastructure.DataSeed;

public class VolunteersSeeder
{
    private readonly IServiceScopeFactory _scopeFactory;

    public VolunteersSeeder(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<VolunteersSeedService>();

        await service.SeedAsync(cancellationToken);
    }
}