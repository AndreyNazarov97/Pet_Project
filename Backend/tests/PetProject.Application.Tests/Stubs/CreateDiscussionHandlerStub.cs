using Moq;
using PetProject.Accounts.Contracts;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.CreateDiscussion;
using PetProject.Discussions.Application.Interfaces;

namespace PetProject.Application.Tests.Stubs;

public class CreateDiscussionHandlerStub : CreateDiscussionHandler
{
    internal Mock<IDiscussionsRepository> DiscussionsRepositoryMock { get; }
    
    internal Mock<IAccountsContract> AccountsContractMock { get; }
    
    public CreateDiscussionHandlerStub(
        Mock<IAccountsContract> accountsContractMock,
        Mock<IDiscussionsRepository> discussionsRepositoryMock) 
        : base(
            accountsContractMock.Object,
            discussionsRepositoryMock.Object
            )
    {
        DiscussionsRepositoryMock = discussionsRepositoryMock;
        AccountsContractMock = accountsContractMock;
    }
}