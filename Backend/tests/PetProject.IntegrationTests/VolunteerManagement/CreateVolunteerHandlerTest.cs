using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData.Creators;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.CreateVolunteer;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class CreateVolunteerHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<CreateVolunteerCommand, Result<Guid, ErrorList>> _sut;

    public CreateVolunteerHandlerTest(VolunteerManagementTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService< IRequestHandler<CreateVolunteerCommand, Result<Guid, ErrorList>>>();
    }

    private static CreateVolunteerCommand Command => new ()
    {
        PhoneNumber = Random.PhoneNumber,
        FullName = DtoCreator.CreateFullNameDto(),
        Description = Random.Words,
        AgeExperience = Random.Experience
    };
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerAlreadyExists()
    {
        // Arrange
        var existingVolunteer = await SeedVolunteer();
        var command = Command with { PhoneNumber = existingVolunteer.PhoneNumber.Value };
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Count.Should().Be(1);
        var error = result.Error.Errors.Single();
        error.Message.Should().Be("Volunteer already exist");
        error.Code.Should().Be("volunteer.already.exist");
    }

    [Fact]
    public async Task ShouldCreateVolunteer()
    {
        var command = Command;

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.PhoneNumber.Value == command.PhoneNumber, CancellationToken.None);
        
        volunteerFromDb.Should().NotBeNull();
        volunteerFromDb!.PhoneNumber.Value.Should().Be(command.PhoneNumber);
        volunteerFromDb!.FullName.Name.Should().Be(command.FullName.Name);
        volunteerFromDb!.FullName.Surname.Should().Be(command.FullName.Surname);
        volunteerFromDb!.FullName.Patronymic.Should().Be(command.FullName.Patronymic);
        volunteerFromDb!.GeneralDescription.Value.Should().Be(command.Description);
        volunteerFromDb!.Experience.Years.Should().Be(command.AgeExperience);
    }
}