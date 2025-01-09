using FileService.Communication.Contracts.Responses;
using FileService.Core;
using FileService.Infrastructure.Providers.Data;
using FileService.Jobs;
using Hangfire;

namespace FileService.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<UploadFileData> _filesData = [];
    private readonly List<FileMetadata> _filesMetaData = [];
    private readonly List<UploadFileResponse> _responses = [];
    
    public IReadOnlyList<UploadFileData> FilesData => _filesData.AsReadOnly();
    public IReadOnlyList<FileMetadata> FilesMetaData => _filesMetaData.AsReadOnly();
    public IReadOnlyList<UploadFileResponse> Responses => _responses.AsReadOnly();
    
    public void Process(IFormFileCollection files, string bucketName)
    {
        foreach (var file in files)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var key = $"{Guid.NewGuid()}{fileExtension}";

            var stream = file.OpenReadStream();
            var fileData = new UploadFileData(stream, bucketName, key, file.ContentType);
            _filesData.Add(fileData);
            
            var fileId = Guid.NewGuid();

            var fileMetadata = new FileMetadata
            {
                Id = fileId,
                Key = key,
                Name = file.FileName,
                BucketName = bucketName,
                ContentType = file.ContentType,
                UploadDate = DateTime.UtcNow
            };
            
            _filesMetaData.Add(fileMetadata);
            _responses.Add(new UploadFileResponse
            {
                Key = key,
                FileId = fileId
            });
            
            BackgroundJob.Schedule<ConsistencyConfirmJob>(
                j => j.Execute(
                    fileMetadata.Id, fileMetadata.BucketName, fileMetadata.Key),
                TimeSpan.FromHours(24));
        }
    }


    public async ValueTask DisposeAsync()
    {
        foreach (var file in _filesData)
        {
            await file.Stream.DisposeAsync();
        }
    }
}