using Microsoft.Extensions.Logging;
using Moq;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;

namespace PetProject.Application.Tests.Stubs;

public class CreateVolunteerRequestHandlerStub : CreateVolunteerRequestHandler
{
    internal Mock<IRequestsRepository> VolunteerRequestsRepositoryMock { get; }
    
    internal Mock<ILogger<CreateVolunteerRequestHandler>> LoggerMock { get; }
    
    public CreateVolunteerRequestHandlerStub(
        Mock<IRequestsRepository> volunteerRequestsRepositoryMock,
        Mock<ILogger<CreateVolunteerRequestHandler>> loggerMock) 
        : base(
            volunteerRequestsRepositoryMock.Object,
            loggerMock.Object)
    {
        VolunteerRequestsRepositoryMock = volunteerRequestsRepositoryMock;
        LoggerMock = loggerMock;
    }
}