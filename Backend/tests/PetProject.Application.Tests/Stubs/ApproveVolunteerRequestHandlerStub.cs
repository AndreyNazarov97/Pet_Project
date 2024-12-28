using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.Approve;

namespace PetProject.Application.Tests.Stubs;

public class ApproveVolunteerRequestHandlerStub : ApproveVolunteerRequestHandler
{
    internal Mock<IVolunteerRequestsRepository> VolunteerRequestsRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<ApproveVolunteerRequestHandler>> LoggerMock { get; }
    
    public ApproveVolunteerRequestHandlerStub(
        Mock<IVolunteerRequestsRepository> volunteerRequestsRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<ApproveVolunteerRequestHandler>> loggerMock) 
        : base(
            volunteerRequestsRepositoryMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteerRequestsRepositoryMock = volunteerRequestsRepositoryMock;
        LoggerMock = loggerMock;
    }
}