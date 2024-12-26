using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.SendForRevision;

namespace PetProject.Application.Tests.Stubs;

public class SendForRevisionHandlerStub : SendForRevisionHandler
{
    internal Mock<IVolunteerRequestsRepository> VolunteerRequestsRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<SendForRevisionHandler>> LoggerMock { get; }
    
    public SendForRevisionHandlerStub(
        Mock<IVolunteerRequestsRepository> volunteerRequestsRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<SendForRevisionHandler>> loggerMock) 
        : base(
            volunteerRequestsRepositoryMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteerRequestsRepositoryMock = volunteerRequestsRepositoryMock;
        LoggerMock = loggerMock;
    }
}