using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeletePet;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class SoftDeletePetHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<SoftDeletePetCommand, UnitResult<ErrorList>> _sut;

    public SoftDeletePetHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<SoftDeletePetCommand, UnitResult<ErrorList>>>();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = new SoftDeletePetCommand {PetId = Guid.NewGuid(), VolunteerId = Guid.NewGuid()};
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetNotFound()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = new SoftDeletePetCommand {PetId = Guid.NewGuid(), VolunteerId = volunteer.Id.Id};
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public async Task Handle_ShouldSoftDeletePet()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = new SoftDeletePetCommand()
        {
            PetId = volunteer.Pets.First().Id.Id, 
            VolunteerId = volunteer.Id.Id
        };
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id, CancellationToken.None);
        var pet = volunteerFromDb!.Pets.First();
        
        pet.IsDeleted.Should().BeTrue();
    }
}