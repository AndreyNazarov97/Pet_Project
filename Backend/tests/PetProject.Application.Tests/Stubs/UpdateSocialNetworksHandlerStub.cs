using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialNetworks;
using PetProject.Accounts.Domain;
using PetProject.Core.Database;

namespace PetProject.Application.Tests.Stubs;

public class UpdateSocialNetworksHandlerStub : UpdateSocialNetworksHandler
{
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<UpdateSocialNetworksHandler>> LoggerMock { get; }
    
    public UpdateSocialNetworksHandlerStub(
        UserManager<User> userManager,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<UpdateSocialNetworksHandler>> loggerMock) 
        : base(
            userManager,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        UnitOfWorkMock = unitOfWorkMock;
        LoggerMock = loggerMock;
    }
}