using Microsoft.Extensions.DependencyInjection;

namespace PetProject.SpeciesManagement.Infrastructure.DataSeed;

public class SpeciesSeeder
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SpeciesSeeder(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<SpeciesSeedService>();

        await service.SeedAsync(cancellationToken);
    }
}