using Moq;
using PetProject.Core.Database.Repository;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteersList;

namespace PetProject.Application.Tests.Stubs;

public class GetVolunteersListHandlerStub : GetVolunteersListHandler
{
    internal Mock<IReadRepository> ReadRepositoryMock { get; }

    public GetVolunteersListHandlerStub(
        Mock<IReadRepository> readRepositoryMock) : 
        base(readRepositoryMock.Object)
    {
        ReadRepositoryMock = readRepositoryMock;
    }
}