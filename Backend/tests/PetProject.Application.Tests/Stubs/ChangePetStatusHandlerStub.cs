using Moq;
using PetProject.Core.Database;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.ChangePetStatus;

namespace PetProject.Application.Tests.Stubs;

public class ChangePetStatusHandlerStub : ChangePetStatusHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    public ChangePetStatusHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock) 
        : base(
            volunteersRepositoryMock.Object,
            unitOfWorkMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
    }
}