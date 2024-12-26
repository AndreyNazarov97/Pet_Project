using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;
using VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

namespace PetProject.Application.Tests.Stubs;

public class TakeVolunteerRequestOnReviewHandlerStub : TakeVolunteerRequestOnReviewHandler
{
    internal Mock<IRequestsRepository> VolunteerRequestsRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<TakeVolunteerRequestOnReviewHandler>> LoggerMock { get; }
    
    public TakeVolunteerRequestOnReviewHandlerStub(
        Mock<IRequestsRepository> volunteerRequestsRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<TakeVolunteerRequestOnReviewHandler>> loggerMock) 
        : base(
            volunteerRequestsRepositoryMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteerRequestsRepositoryMock = volunteerRequestsRepositoryMock;
        LoggerMock = loggerMock;
    }
}