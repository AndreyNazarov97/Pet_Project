using CSharpFunctionalExtensions;
using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Tests.Extensions;

public static class FileProviderExtensions
{
    public static void SetupUploadFiles(
        this Mock<IFileProvider> mock,
        Result<IReadOnlyCollection<FilePath>,ErrorList> result)
    {
        mock.Setup(fp => fp.UploadFiles(
            It.IsAny<IEnumerable<FileDataDto>>(),
            It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(result);
    }
}