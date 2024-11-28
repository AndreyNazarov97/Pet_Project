using CSharpFunctionalExtensions;
using Moq;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.Providers;

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