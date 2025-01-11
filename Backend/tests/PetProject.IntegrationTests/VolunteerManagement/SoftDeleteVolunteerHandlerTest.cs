using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeleteVolunteer;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class SoftDeleteVolunteerHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<SoftDeleteVolunteerCommand, UnitResult<ErrorList>> _sut;

    public SoftDeleteVolunteerHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<SoftDeleteVolunteerCommand, UnitResult<ErrorList>>>();
    }

    private static SoftDeleteVolunteerCommand Command => new()
    {
        VolunteerId = Guid.NewGuid()
    };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var command = Command;

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }

    [Fact]
    public async Task Handle_ShouldDeleteVolunteerSuccessfully_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = Command with { VolunteerId = volunteer.Id.Id };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == command.VolunteerId, CancellationToken.None);

        volunteerFromDb!.IsDeleted.Should().BeTrue();
    }
}