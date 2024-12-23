using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetProject.VolunteerManagement.Infrastructure.Services;

namespace PetProject.VolunteerManagement.Infrastructure.BackgroundServices;

public class DeletedEntityCleanupService : BackgroundService
{
    private const int DelayHours = 24;
    
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DeletedEntityCleanupService> _logger;

    public DeletedEntityCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<DeletedEntityCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeletedEntityCleanupService is starting");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope =  _scopeFactory.CreateAsyncScope();
        
            var deleteExpiredPetsService = scope.ServiceProvider.GetRequiredService<DeleteExpiredPetsService>();
            var deleteExpiredVolunteersService = scope.ServiceProvider.GetRequiredService<DeleteExpiredVolunteersService>();
            
            await deleteExpiredPetsService.DeleteExpiredPets();
            _logger.LogInformation("Deleted expired pets");
            
            await deleteExpiredVolunteersService.Process();
            _logger.LogInformation("Deleted expired volunteers");
            
            await Task.Delay(TimeSpan.FromHours(DelayHours), stoppingToken);
        }
    }
}