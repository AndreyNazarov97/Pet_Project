using Moq;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.SpeciesManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.UpdatePet;

namespace PetProject.Application.Tests.Stubs;

public class UpdatePetHandlerStub : UpdatePetHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<IReadRepository> ReadRepositoryMock { get; }
    
    public UpdatePetHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IReadRepository> readRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock)
        : base(
            volunteersRepositoryMock.Object,
            readRepositoryMock.Object,
            unitOfWorkMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        ReadRepositoryMock = readRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
    }
}