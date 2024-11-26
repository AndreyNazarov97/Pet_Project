using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.UpdatePet;

namespace PetProject.Application.Tests.Stubs;

public class UpdatePetHandlerStub : UpdatePetHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ISpeciesRepository> SpeciesRepositoryMock { get; }
    
    public UpdatePetHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<ISpeciesRepository> speciesRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock)
        : base(
            volunteersRepositoryMock.Object,
            speciesRepositoryMock.Object,
            unitOfWorkMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        SpeciesRepositoryMock = speciesRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
    }
}