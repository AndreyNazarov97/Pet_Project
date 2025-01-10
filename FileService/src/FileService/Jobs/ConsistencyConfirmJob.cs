using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using Hangfire;

namespace FileService.Jobs;

public class ConsistencyConfirmJob(
    IFilesRepository filesRepository,
    IFileProvider fileProvider,
    ILogger<ConsistencyConfirmJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(Guid fileId, string bucketName, string key)
    {
        try
        {
            logger.LogInformation("Start ConsistencyConfirmJob with {fileId} and {key}", fileId, key);

            var mongoData = await filesRepository.GetById(fileId);

            if (mongoData is null)
            {
                logger.LogWarning("MongoDB record not found for fileId: {fileId}." +
                                  " Deleting file from cloud storage.", fileId);
                await fileProvider.DeleteFile(bucketName, key);
                return;
            }

            if (key != mongoData.Key)
            {
                logger.LogWarning("Metadata key does not match MongoDB data." +
                                  " Deleting file from cloud storage and MongoDB record.");

                await fileProvider.DeleteFile(bucketName, key);
                await filesRepository.DeleteRangeAsync([fileId]);
            }

            logger.LogInformation("End ConfirmConsistencyJob");
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot check consistency, because " + ex.Message);
        }
    }
}