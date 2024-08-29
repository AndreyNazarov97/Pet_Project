using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Application.Abstractions;

public interface IMinioProvider
{
    public Task<Result<string, Error>> DownloadFile(string bucketName, string fileName);


    public  Task<UnitResult<Error>> UploadFile(Stream stream, string bucketName, string fileName, CancellationToken cancellationToken = default);


    public Task<UnitResult<Error>> DeleteFile(string bucketName, string fileName, CancellationToken cancellationToken = default);

}