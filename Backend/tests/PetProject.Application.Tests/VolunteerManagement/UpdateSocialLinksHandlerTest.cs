using FluentAssertions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.SharedTestData;

namespace PetProject.Application.Tests.VolunteerManagement;

public class UpdateSocialLinksHandlerTest
{
    private static UpdateSocialLinksCommand Command => new()
    {
        VolunteerId = Guid.NewGuid(),
        SocialLinks = new List<SocialLinkDto>()
        {
            new("Title 1", "http://example.com"),
            new("Title 2", "http://example2.com")
        }
    };


    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var command = Command;
        var handler = StubFactory.CreateUpdateSocialLinksHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(command.VolunteerId),
            Errors.General.NotFound(command.VolunteerId));
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
        handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
                It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateSocialLinks_WhenVolunteerExists()
    {
        // Arrange
        var volunteer = TestData.Volunteer;
        var command = Command with { VolunteerId = volunteer.Id.Id };

        var handler = StubFactory.CreateUpdateSocialLinksHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            volunteer.Id,
            volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.UnitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        volunteer.SocialLinks.Count.Should().Be(2);
    }
}