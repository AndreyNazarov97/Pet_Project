using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData.Creators;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.UpdateVolunteer;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class UpdateVolunteerHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<UpdateVolunteerCommand, Result<Guid, ErrorList>> _sut;

    public UpdateVolunteerHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<UpdateVolunteerCommand, Result<Guid, ErrorList>>>();
    }

    private static UpdateVolunteerCommand UpdateVolunteerCommand => new()
    {
        VolunteerId = Guid.NewGuid(),
        FullName = DtoCreator.CreateFullNameDto(),
        Description = Random.Words,
        AgeExperience = Random.Experience,
        PhoneNumber = Random.PhoneNumber
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = UpdateVolunteerCommand;
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldUpdateVolunteer_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = UpdateVolunteerCommand with { VolunteerId = volunteer.Id.Id };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == command.VolunteerId, CancellationToken.None);

        volunteerFromDb.FullName.Name.Should().Be(command.FullName.Name);
        volunteerFromDb.FullName.Surname.Should().Be(command.FullName.Surname);
        volunteerFromDb.FullName.Patronymic.Should().Be(command.FullName.Patronymic);
        volunteerFromDb.GeneralDescription.Value.Should().Be(command.Description);
        volunteerFromDb.Experience.Years.Should().Be(command.AgeExperience);
        volunteerFromDb.PhoneNumber.Value.Should().Be(command.PhoneNumber);
    }
}