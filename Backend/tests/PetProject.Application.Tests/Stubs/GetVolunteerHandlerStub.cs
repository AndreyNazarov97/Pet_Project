using Moq;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.GetVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class GetVolunteerHandlerStub : GetVolunteerHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }

    public GetVolunteerHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock) : 
        base(volunteersRepositoryMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
    }
}