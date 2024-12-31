using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using PetProject.Discussions.Contracts;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;
using VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

namespace PetProject.Application.Tests.Stubs;

public class TakeVolunteerRequestOnReviewHandlerStub : TakeVolunteerRequestOnReviewHandler
{
    internal Mock<IVolunteerRequestsRepository> VolunteerRequestsRepositoryMock { get; }
    
    internal Mock<IDiscussionContract> DiscussionContractMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<TakeVolunteerRequestOnReviewHandler>> LoggerMock { get; }
    
    public TakeVolunteerRequestOnReviewHandlerStub(
        Mock<IDiscussionContract> discussionContractMock,
        Mock<IVolunteerRequestsRepository> volunteerRequestsRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<TakeVolunteerRequestOnReviewHandler>> loggerMock) 
        : base(
            discussionContractMock.Object,
            volunteerRequestsRepositoryMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteerRequestsRepositoryMock = volunteerRequestsRepositoryMock;
        LoggerMock = loggerMock;
        DiscussionContractMock = discussionContractMock;
        UnitOfWorkMock = unitOfWorkMock;
    }
}