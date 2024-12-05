using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.Options;

namespace PetProject.Accounts.Infrastructure.DataSeed;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AccountsSeeder(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<AccountsSeedService>();

        await service.SeedAsync(cancellationToken);
    }
    
}