using FluentAssertions;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Core.Dtos;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetVolunteersList;

namespace PetProject.Application.Tests.VolunteerManagement;

public class GetVolunteersListHandlerTest
{
    private static GetVolunteersListQuery Query => new()
    {
        Offset = 0,
        Limit = 0
    };

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoVolunteers()
    {
        //Arrange
        var query = Query;
        var handler = StubFactory.CreateGetVolunteersListHandlerStub();

        //Act
        handler.ReadRepositoryMock.SetupQueryVolunteer([]);
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnVolunteers_WhenVolunteersExist()
    {
        //Arrange
        var query = Query;
        var handler = StubFactory.CreateGetVolunteersListHandlerStub();
        var volunteers = new List<VolunteerDto>()
        {
            TestData.VolunteerDto,
            TestData.VolunteerDto
        };

        //Act
        handler.ReadRepositoryMock.SetupQueryVolunteer(volunteers.ToArray());
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(2);
    }
}