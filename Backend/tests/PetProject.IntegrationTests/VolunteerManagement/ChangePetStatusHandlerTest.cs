using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.ChangePetStatus;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class ChangePetStatusHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<ChangePetStatusCommand, UnitResult<ErrorList>> _sut;

    private static ChangePetStatusCommand Command => new()
    {
        VolunteerId = Guid.NewGuid(),
        PetId = Guid.NewGuid(),
        HelpStatus = Random.HelpStatus
    };
    
    public ChangePetStatusHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<ChangePetStatusCommand, UnitResult<ErrorList>>>();
    }
    
     [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        //Arrange
        var command = Command;
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPetNotFound()
    {
        //Arrange
        var volunteer = await SeedVolunteer();
        var command = Command with{VolunteerId = volunteer.Id.Id};
        
        //Act
        var result = await  _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenStatusChanged()
    {
        //Arrange
        var volunteer = await SeedVolunteer();
        var pet = volunteer.Pets.First();
        var command = Command with{VolunteerId = volunteer.Id.Id, PetId = pet.Id.Id};
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id, CancellationToken.None);
        var petFromDb = volunteerFromDb!.Pets.First();
        
        petFromDb.HelpStatus.Should().Be(command.HelpStatus);
    }
}