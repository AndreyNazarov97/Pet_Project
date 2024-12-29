using Moq;
using PetProject.Core.Database;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.CloseDiscussion;
using PetProject.Discussions.Application.Interfaces;

namespace PetProject.Application.Tests.Stubs;

public class CloseDiscussionHandlerStub : CloseDiscussionHandler
{
    internal Mock<IDiscussionsRepository> DiscussionsRepositoryMock { get; }

    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }

    public CloseDiscussionHandlerStub(
        Mock<IDiscussionsRepository> discussionsRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock)
        : base(
            discussionsRepositoryMock.Object,
            unitOfWorkMock.Object
        )
    {
        DiscussionsRepositoryMock = discussionsRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
    }
}