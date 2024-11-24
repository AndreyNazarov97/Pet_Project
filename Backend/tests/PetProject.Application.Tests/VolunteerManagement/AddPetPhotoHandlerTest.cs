using FluentAssertions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.AddPetPhoto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Tests.VolunteerManagement;

public class AddPetPhotoHandlerTest
{
    private readonly AddPetPhotoCommand _addPetPhotoCommand = new AddPetPhotoCommand
    {
        VolunteerId = Guid.NewGuid(),
        PetId = Guid.NewGuid(),
        Photos = new List<FileDto>
        {
            TestData.FileDto,
            TestData.FileDto
        }
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var command = _addPetPhotoCommand;

        var handler = StubFactory.CreateAddPetPhotoHandlerStub();

        // Act
        handler.UnitOfWorkMock.SetupTransaction();
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));
        handler.FileProviderMock.SetupUploadFiles(Errors.Minio.CouldNotUploadFile().ToErrorList());
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(command.VolunteerId));
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.FileProviderMock.Verify(provider => provider
                .UploadFiles(
                    It.IsAny<IEnumerable<FileDataDto>>(),
                    It.IsAny<CancellationToken>()),
            Times.Never);
        handler.UnitOfWorkMock.Verify(u => u
                .BeginTransactionAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetDoesNotExist()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = _addPetPhotoCommand with
        {
            VolunteerId = volunteer.Id.Id,
        };

        var handler = StubFactory.CreateAddPetPhotoHandlerStub();

        // Act
        handler.UnitOfWorkMock.SetupTransaction();
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.FileProviderMock.SetupUploadFiles(Errors.Minio.CouldNotUploadFile().ToErrorList());
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(command.PetId));
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.FileProviderMock.Verify(provider => provider
                .UploadFiles(
                    It.IsAny<IEnumerable<FileDataDto>>(),
                    It.IsAny<CancellationToken>()),
            Times.Never);
        handler.UnitOfWorkMock.Verify(u => u
                .BeginTransactionAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenFilesNotUploaded()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        var command = _addPetPhotoCommand with
        {
            VolunteerId = volunteer.Id.Id,
            PetId = pet.Id.Id
        };

        var handler = StubFactory.CreateAddPetPhotoHandlerStub();

        // Act
        handler.UnitOfWorkMock.SetupTransaction();
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.FileProviderMock.SetupUploadFiles(Errors.Minio.CouldNotUploadFile().ToErrorList());
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.Minio.CouldNotUploadFile());
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.FileProviderMock.Verify(provider => provider
                .UploadFiles(
                    It.IsAny<IEnumerable<FileDataDto>>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .BeginTransactionAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldAddPetPhoto_WhenPetExists()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        var command = _addPetPhotoCommand with
        {
            VolunteerId = volunteer.Id.Id,
            PetId = pet.Id.Id
        };

        var filePath1 = FilePath.Create("path1", ".jpg").Value;
        var filePath2 = FilePath.Create("path2", ".jpg").Value;
        var filePaths = new List<FilePath> { filePath1, filePath2 }.AsReadOnly();

        var handler = StubFactory.CreateAddPetPhotoHandlerStub();

        // Act
        handler.UnitOfWorkMock.SetupTransaction();
        handler.VolunteersRepositoryMock.SetupGetById(volunteer.Id, volunteer);
        handler.FileProviderMock.SetupUploadFiles(filePaths);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo
                .GetById(
                    It.IsAny<VolunteerId>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.FileProviderMock.Verify(provider => provider
                .UploadFiles(
                    It.IsAny<IEnumerable<FileDataDto>>(),
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .BeginTransactionAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());
        handler.UnitOfWorkMock.Verify(u => u
                .SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once());

        result.Value.Length.Should().Be(2);
        result.Value[0].Should().BeEquivalentTo(filePath1);
        result.Value[1].Should().BeEquivalentTo(filePath2);
    }
}