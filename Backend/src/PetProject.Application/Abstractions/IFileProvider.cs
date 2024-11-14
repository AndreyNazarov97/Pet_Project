using CSharpFunctionalExtensions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Abstractions;

public interface IFileProvider
{
    public Task<Result<string, Error>> DownloadFile(FileMetaDataDto fileMetaData);


    public  Task<Result<IReadOnlyCollection<FilePath>,Error>> UploadFiles(IEnumerable<FileDataDto>  filesData, CancellationToken cancellationToken = default);


    public Task<UnitResult<Error>> DeleteFile(FileMetaDataDto fileMetaData, CancellationToken cancellationToken = default);

}