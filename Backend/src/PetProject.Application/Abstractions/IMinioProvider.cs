using PetProject.Domain.Shared;

namespace PetProject.Application.Abstractions;

public interface IMinioProvider
{
    public Task<Result<string>> DownloadFile(string bucketName, string fileName);


    public  Task<Result> UploadFile(Stream stream, string bucketName, string fileName, CancellationToken cancellationToken = default);


    public Task<Result> DeleteFile(string bucketName, string fileName, CancellationToken cancellationToken = default);

}