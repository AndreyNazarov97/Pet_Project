using FluentAssertions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.GetVolunteersList;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.Tests.VolunteerManagement;

public class GetVolunteersListHandlerTest
{
    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoVolunteers()
    {
        //Arrange
        var query = new GetVolunteersListQuery
        {
            Offset = 0,
            Limit = 0
        };

        var handler = StubFactory.CreateGetVolunteersListHandlerStub();
    
        //Act
        handler.VolunteersRepositoryMock.SetupQuery([]);
        var result = await handler.Handle(query, CancellationToken.None);
    
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnVolunteers_WhenVolunteersExist()
    {
        //Arrange
        var query = new GetVolunteersListQuery
        {
            Offset = 0,
            Limit = 0
        };

        var handler = StubFactory.CreateGetVolunteersListHandlerStub();
        var volunteers = new List<VolunteerDto>()
        {
            TestData.VolunteerDto,
            TestData.VolunteerDto
        };
    
        //Act
        handler.VolunteersRepositoryMock.SetupQuery(volunteers.ToArray());
        var result = await handler.Handle(query, CancellationToken.None);
    
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(2);
    }
}