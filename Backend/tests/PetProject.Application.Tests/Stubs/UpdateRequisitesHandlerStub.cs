using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Accounts.Application.AccountManagement.Commands.UpdateRequisites;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Core.Database;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.Application.Tests.Stubs;

public class UpdateRequisitesHandlerStub : UpdateRequisitesHandler
{
    internal Mock<IAccountManager> AccountManagerMock { get; }

    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }

    internal Mock<ILogger<UpdateRequisitesHandler>> LoggerMock { get; }

    public UpdateRequisitesHandlerStub(
        UserManager<User> userManager,
        Mock<IAccountManager> accountManagerMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<UpdateRequisitesHandler>> loggerMock)
        : base(
            userManager,
            accountManagerMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        AccountManagerMock = accountManagerMock;
        UnitOfWorkMock = unitOfWorkMock;
        LoggerMock = loggerMock;
    }
}