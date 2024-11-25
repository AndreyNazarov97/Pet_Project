using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.ChangePetStatus;

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