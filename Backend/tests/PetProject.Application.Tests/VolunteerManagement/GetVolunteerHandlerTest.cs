using FluentAssertions;
using Moq;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetVolunteer;

namespace PetProject.Application.Tests.VolunteerManagement;

public class GetVolunteerHandlerTest
{
    private static GetVolunteerQuery Query => new()
    {
        VolunteerId = Guid.NewGuid()
    };
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        //Arrange
        var query = Query;
        var handler = StubFactory.CreateGetVolunteerHandlerStub();
    
        //Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(query.VolunteerId),
            Errors.General.NotFound(query.VolunteerId));
        var result = await handler.Handle(query);
    
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnVolunteer_WhenVolunteerFound()
    {
        //Arrange
        var volunteer = TestData.Volunteer;
        var query = Query with {VolunteerId = volunteer.Id.Id};

        var handler = StubFactory.CreateGetVolunteerHandlerStub();
    
        //Act
        handler.VolunteersRepositoryMock.SetupGetById(
            volunteer.Id,
            volunteer);
        var result = await handler.Handle(query);
    
        //Assert
        result.IsSuccess.Should().BeTrue();
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
        
        var volunteerResult = result.Value;

        volunteerResult.FullName.Name.Should().Be(volunteer.FullName.Name);
        volunteerResult.FullName.Surname.Should().Be(volunteer.FullName.Surname);
        volunteerResult.FullName.Patronymic.Should().Be(volunteer.FullName.Patronymic);
        volunteerResult.GeneralDescription.Should().Be(volunteer.GeneralDescription.Value);
        volunteerResult.AgeExperience.Should().Be(volunteer.Experience.Years);
        volunteerResult.PhoneNumber.Should().Be(volunteer.PhoneNumber.Value);
    }
}