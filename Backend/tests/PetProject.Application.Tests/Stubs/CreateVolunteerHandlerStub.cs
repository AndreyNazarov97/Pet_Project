using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.CreateVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class CreateVolunteerHandlerStub : CreateVolunteerHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<ILogger<CreateVolunteerHandler>> LoggerMock { get; }
    
    public CreateVolunteerHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<ILogger<CreateVolunteerHandler>> loggerMock) 
        : base(
            volunteersRepositoryMock.Object,
            loggerMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        LoggerMock = loggerMock;
    }
}