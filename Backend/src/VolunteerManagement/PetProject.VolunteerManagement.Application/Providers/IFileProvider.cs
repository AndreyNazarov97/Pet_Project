using CSharpFunctionalExtensions;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.VolunteerManagement.Application.Providers;

public interface IFileProvider
{
    public Task<Result<string, ErrorList>> DownloadFile(FileMetaDataDto fileMetaData);

    public Task<Result<IReadOnlyCollection<FilePath>, ErrorList>> UploadFiles(
        IEnumerable<FileDataDto> filesData, CancellationToken cancellationToken = default);

    public Result<IReadOnlyCollection<string>, ErrorList> GetFiles(string bucketName);

    public Task<UnitResult<ErrorList>> DeleteFile(
        FileMetaDataDto fileMetaData, CancellationToken cancellationToken = default);
}