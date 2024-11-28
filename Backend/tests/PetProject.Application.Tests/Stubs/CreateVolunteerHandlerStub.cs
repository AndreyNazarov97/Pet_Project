using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database.Repository;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.CreateVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class CreateVolunteerHandlerStub : CreateVolunteerHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IReadRepository> ReadRepositoryMock { get; }
    
    internal Mock<ILogger<CreateVolunteerHandler>> LoggerMock { get; }
    
    public CreateVolunteerHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IReadRepository> readRepositoryMock,
        Mock<ILogger<CreateVolunteerHandler>> loggerMock) 
        : base(
            volunteersRepositoryMock.Object,
            readRepositoryMock.Object,
            loggerMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        ReadRepositoryMock = readRepositoryMock;
        LoggerMock = loggerMock;
    }
}