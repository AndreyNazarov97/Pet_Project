using Moq;
using PetProject.Core.Database;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;
using PetProject.Discussions.Application.Interfaces;

namespace PetProject.Application.Tests.Stubs;

public class SendMessageHandlerStub : SendMessageHandler
{
    internal Mock<IDiscussionsRepository> DiscussionsRepositoryMock { get; }

    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }

    public SendMessageHandlerStub(
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