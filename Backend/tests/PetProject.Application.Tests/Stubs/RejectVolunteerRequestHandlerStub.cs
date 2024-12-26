using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.Reject;

namespace PetProject.Application.Tests.Stubs;

public class RejectVolunteerRequestHandlerStub: RejectVolunteerRequestHandler
{
    internal Mock<IRequestsRepository> VolunteerRequestsRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<RejectVolunteerRequestHandler>> LoggerMock { get; }
    
    public RejectVolunteerRequestHandlerStub(
        Mock<IRequestsRepository> volunteerRequestsRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<RejectVolunteerRequestHandler>> loggerMock) 
        : base(
            volunteerRequestsRepositoryMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteerRequestsRepositoryMock = volunteerRequestsRepositoryMock;
        LoggerMock = loggerMock;
    }
}