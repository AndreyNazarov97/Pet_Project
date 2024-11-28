using Moq;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteer;

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