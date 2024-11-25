using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.UpdateRequisites;

namespace PetProject.Application.Tests.Stubs;

public class UpdateRequisitesHandlerStub : UpdateRequisitesHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<UpdateRequisitesHandler>> LoggerMock { get; }
    
    public UpdateRequisitesHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<UpdateRequisitesHandler>> loggerMock) 
        : base(
            volunteersRepositoryMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
        LoggerMock = loggerMock;
    }
}