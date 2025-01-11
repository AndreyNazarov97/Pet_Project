using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.AddPetPhoto;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class AddPetPhotoHandlerTest : VolunteerManagementTestsBase
{
    private readonly IRequestHandler<AddPetPhotoCommand, Result<FilePath[], ErrorList>> _sut;

    private static AddPetPhotoCommand AddPetPhotoCommand => new AddPetPhotoCommand
    {
        VolunteerId = Guid.NewGuid(),
        PetId = Guid.NewGuid(),
        Keys = [$"{Guid.NewGuid()}.jpg", $"{Guid.NewGuid()}.jpg"]
    };

    public AddPetPhotoHandlerTest(BaseTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<IRequestHandler<AddPetPhotoCommand, Result<FilePath[], ErrorList>>>();
    }
    
    [Fact]
    public async Task ShouldReturnError_WhenVolunteerDoesNotExist()
    {
        // Arrange
        var command = AddPetPhotoCommand;

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(command.VolunteerId));
    }
    
    [Fact]
    public async Task ShouldReturnError_WhenPetDoesNotExist()
    {
        // Arrange
        var volunteer = await SeedVolunteer();
        var command = AddPetPhotoCommand with
        {
            VolunteerId = volunteer.Id.Id,
        };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain(Errors.General.NotFound(command.PetId));
    }

    [Fact]
    public async Task ShouldAddPetPhoto_WhenPetExists()
    {
        // Arrange 
        var volunteer = await SeedVolunteer();
        var command = AddPetPhotoCommand with
        {
            VolunteerId = volunteer.Id.Id, 
            PetId = volunteer.Pets.First().Id.Id
        };
        
        var filePath  = FilePath.Create(command.Keys.First()).Value;
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.First().Path.Should().Be(filePath.Path);
        
        var volunteerFromDb = await _volunteerDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id, CancellationToken.None);
        
        volunteerFromDb.Should().NotBeNull();
        volunteerFromDb!.Pets.Should().HaveCount(1);
        volunteerFromDb.Pets.First().PetPhotoList.Should().HaveCount(2);
        volunteerFromDb.Pets.First().PetPhotoList.Should().Contain(p => p.Path.Path == filePath.Path);
    }
    
}