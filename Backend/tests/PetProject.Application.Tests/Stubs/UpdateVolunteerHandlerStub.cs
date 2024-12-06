using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.UpdateVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class UpdateVolunteerHandlerStub : UpdateVolunteerHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<UpdateVolunteerHandler>> LoggerMock { get; }
    
    public UpdateVolunteerHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<UpdateVolunteerHandler>> loggerMock) 
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