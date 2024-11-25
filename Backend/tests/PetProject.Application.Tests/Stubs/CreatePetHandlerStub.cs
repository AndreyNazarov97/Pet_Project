using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.CreatePet;

namespace PetProject.Application.Tests.Stubs;

public class CreatePetHandlerStub : CreatePetHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ISpeciesRepository> SpeciesRepositoryMock { get; }
    
    public CreatePetHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ISpeciesRepository> speciesRepositoryMock) 
        : base(
            volunteersRepositoryMock.Object,
            speciesRepositoryMock.Object,
            unitOfWorkMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
        SpeciesRepositoryMock = speciesRepositoryMock;
    }
}