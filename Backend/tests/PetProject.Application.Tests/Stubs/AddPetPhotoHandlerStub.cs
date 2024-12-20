using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using PetProject.Core.Dtos;
using PetProject.Core.Messaging;
using PetProject.VolunteerManagement.Application.Providers;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.AddPetPhoto;

namespace PetProject.Application.Tests.Stubs;

public class AddPetPhotoHandlerStub : AddPetPhotoHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }

    internal Mock<IFileProvider> FileProviderMock { get; }
    
    internal Mock<IMessageQueue<IEnumerable<FileMetaDataDto>>> MessageQueueMock { get; }

    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }

    internal Mock<ILogger<AddPetPhotoHandler>> LoggerMock { get; }

    public AddPetPhotoHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IFileProvider> fileProviderMock,
        Mock<IMessageQueue<IEnumerable<FileMetaDataDto>>> messageQueueMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<AddPetPhotoHandler>> loggerMock)
        : base(
            volunteersRepositoryMock.Object,
            fileProviderMock.Object,
            messageQueueMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        FileProviderMock = fileProviderMock;
        UnitOfWorkMock = unitOfWorkMock;
        LoggerMock = loggerMock;
    }
}