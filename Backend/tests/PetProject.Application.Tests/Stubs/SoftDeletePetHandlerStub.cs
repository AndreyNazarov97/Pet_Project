using Moq;
using PetProject.Core.Database;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.SoftDeletePet;

namespace PetProject.Application.Tests.Stubs;

public class SoftDeletePetHandlerStub : SoftDeletePetHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    public SoftDeletePetHandlerStub(
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