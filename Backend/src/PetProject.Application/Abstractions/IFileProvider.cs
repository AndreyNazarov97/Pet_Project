using CSharpFunctionalExtensions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.Abstractions;

public interface IFileProvider
{
    public Task<Result<string, Error>> DownloadFile(FileMetaDataDto fileMetaData);


    public  Task<UnitResult<Error>> UploadFile(FileDataDto fileData, CancellationToken cancellationToken = default);


    public Task<UnitResult<Error>> DeleteFile(FileMetaDataDto fileMetaData, CancellationToken cancellationToken = default);

}