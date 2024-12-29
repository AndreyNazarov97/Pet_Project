using Moq;
using PetProject.Core.Database;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;
using PetProject.Discussions.Application.Interfaces;

namespace PetProject.Application.Tests.Stubs;

public class UpdateMessageHandlerStub : UpdateMessageHandler
{
    internal Mock<IDiscussionsRepository> DiscussionsRepositoryMock { get; }

    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }

    public UpdateMessageHandlerStub(
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