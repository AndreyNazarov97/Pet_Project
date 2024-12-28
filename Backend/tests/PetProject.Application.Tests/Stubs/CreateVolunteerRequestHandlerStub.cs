using Microsoft.Extensions.Logging;
using Moq;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;

namespace PetProject.Application.Tests.Stubs;

public class CreateVolunteerRequestHandlerStub : CreateVolunteerRequestHandler
{
    internal Mock<IVolunteerRequestsRepository> VolunteerRequestsRepositoryMock { get; }
    
    internal Mock<ILogger<CreateVolunteerRequestHandler>> LoggerMock { get; }
    
    public CreateVolunteerRequestHandlerStub(
        Mock<IVolunteerRequestsRepository> volunteerRequestsRepositoryMock,
        Mock<ILogger<CreateVolunteerRequestHandler>> loggerMock) 
        : base(
            volunteerRequestsRepositoryMock.Object,
            loggerMock.Object)
    {
        VolunteerRequestsRepositoryMock = volunteerRequestsRepositoryMock;
        LoggerMock = loggerMock;
    }
}