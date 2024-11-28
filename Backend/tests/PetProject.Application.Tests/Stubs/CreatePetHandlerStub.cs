using Moq;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.CreatePet;

namespace PetProject.Application.Tests.Stubs;

public class CreatePetHandlerStub : CreatePetHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<IReadRepository> ReadRepositoryMock { get; }
    
    public CreatePetHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IReadRepository> readRepositoryMock) 
        : base(
            volunteersRepositoryMock.Object,
            readRepositoryMock.Object,
            unitOfWorkMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
        ReadRepositoryMock = readRepositoryMock;
    }
}