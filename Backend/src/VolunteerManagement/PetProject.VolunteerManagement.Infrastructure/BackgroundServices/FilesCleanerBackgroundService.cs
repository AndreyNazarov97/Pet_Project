using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetProject.Core.Dtos;
using PetProject.Core.Messaging;
using PetProject.VolunteerManagement.Application.Providers;

namespace PetProject.VolunteerManagement.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly IMessageQueue<IEnumerable<FileMetaDataDto>> _messageQueue;
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public FilesCleanerBackgroundService(
        IServiceScopeFactory scopeFactory,
        IMessageQueue<IEnumerable<FileMetaDataDto>> messageQueue,
        ILogger<FilesCleanerBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _messageQueue = messageQueue;
        _logger = logger;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope =  _scopeFactory.CreateAsyncScope();
        
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();
        
        _logger.LogInformation("FilesCleanerBackgroundService is starting");
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            var filesMetadata = await _messageQueue.ReadAsync(stoppingToken);

            foreach (var fileMetaDataDto in filesMetadata)
            {
                await fileProvider.DeleteFile(fileMetaDataDto, stoppingToken);
                _logger.LogInformation("File {FileName} deleted from Minio", fileMetaDataDto.ObjectName);
            }
        }

        await Task.CompletedTask;
    }
}