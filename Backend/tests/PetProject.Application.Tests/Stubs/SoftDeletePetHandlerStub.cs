using Moq;
using PetProject.Core.Database;
using PetProject.SharedKernel.Interfaces;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeletePet;

namespace PetProject.Application.Tests.Stubs;

public class SoftDeletePetHandlerStub : SoftDeletePetHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IDateTimeProvider> DateTimeProviderMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    public SoftDeletePetHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IDateTimeProvider> dateTimeProviderMock,
        Mock<IUnitOfWork> unitOfWorkMock) 
        : base(
            volunteersRepositoryMock.Object,
            dateTimeProviderMock.Object,
            unitOfWorkMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
    }
}