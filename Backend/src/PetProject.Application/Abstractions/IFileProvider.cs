using CSharpFunctionalExtensions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Abstractions;

public interface IFileProvider
{
    public Task<Result<string, ErrorList>> DownloadFile(FileMetaDataDto fileMetaData);
    
    public  Task<Result<IReadOnlyCollection<FilePath>,ErrorList>> UploadFiles(
        IEnumerable<FileDataDto>  filesData, CancellationToken cancellationToken = default);
    public Task<Result<IReadOnlyCollection<string>,ErrorList>> GetFiles(string bucketName);
    public Task<UnitResult<ErrorList>> DeleteFile(
        FileMetaDataDto fileMetaData, CancellationToken cancellationToken = default);

}