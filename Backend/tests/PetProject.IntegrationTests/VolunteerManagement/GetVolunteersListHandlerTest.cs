using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetVolunteersList;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class GetVolunteersListHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<GetVolunteersListQuery, Result<VolunteerDto[], ErrorList>> _sut;

    public GetVolunteersListHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<GetVolunteersListQuery, Result<VolunteerDto[], ErrorList>>>();
    }

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

        //Act
        var result = await _sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnVolunteers_WhenVolunteersExist()
    {
        //Arrange
        var query = Query;
        await SeedVolunteer();
        await SeedVolunteer();

        //Act
        var result = await _sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(2);
    }

    [Fact]
    public async Task ShouldReturnVolunteersWithPagination_WhenVolunteersExist()
    {
        //Arrange
        var query = Query with { Offset = 1, Limit = 1 };
        await SeedVolunteer();
        await SeedVolunteer();

        //Act
        var result = await _sut.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(1);
    }
}