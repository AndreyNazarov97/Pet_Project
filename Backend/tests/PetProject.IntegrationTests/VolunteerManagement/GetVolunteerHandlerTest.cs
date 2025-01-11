using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetVolunteer;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class GetVolunteerHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<GetVolunteerQuery, Result<VolunteerDto, ErrorList>> _sut;

    public GetVolunteerHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<GetVolunteerQuery, Result<VolunteerDto, ErrorList>>>();
    }

    private static GetVolunteerQuery Query => new()
    {
        VolunteerId = Guid.NewGuid()
    };
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        //Arrange
        var query = Query;
    
        //Act
        var result = await _sut.Handle(query, CancellationToken.None);
    
        //Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnVolunteer_WhenVolunteerFound()
    {
        //Arrange
        var volunteer = await SeedVolunteer();
        var query = Query with {VolunteerId = volunteer.Id.Id};
    
        //Act
        var result = await _sut.Handle(query, CancellationToken.None);
    
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerResult = result.Value;

        volunteerResult.FullName.Name.Should().Be(volunteer.FullName.Name);
        volunteerResult.FullName.Surname.Should().Be(volunteer.FullName.Surname);
        volunteerResult.FullName.Patronymic.Should().Be(volunteer.FullName.Patronymic);
        volunteerResult.GeneralDescription.Should().Be(volunteer.GeneralDescription.Value);
        volunteerResult.AgeExperience.Should().Be(volunteer.Experience.Years);
        volunteerResult.PhoneNumber.Should().Be(volunteer.PhoneNumber.Value);
    }
}