using Moq;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.GetVolunteersList;

namespace PetProject.Application.Tests.Stubs;

public class GetVolunteersListHandlerStub : GetVolunteersListHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }

    public GetVolunteersListHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock) : 
        base(volunteersRepositoryMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
    }
}