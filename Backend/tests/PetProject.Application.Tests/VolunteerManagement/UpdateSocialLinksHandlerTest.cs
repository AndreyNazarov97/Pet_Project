using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Tests.Extensions;
using PetProject.Application.Tests.Stubs;
using PetProject.Application.VolunteersManagement.UpdateRequisites;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Tests.VolunteerManagement;

public class UpdateSocialLinksHandlerTest
{
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    {
        // Arrange
        var volunteerId = Guid.NewGuid();
        var command = new UpdateSocialLinksCommand()
        {
            Id = volunteerId,
            SocialLinks = new List<SocialLinkDto>()
            {
                new("Title 1", "http://example.com"),
                new ("Title 2", "http://example2.com")
            }
        };

        var handler = StubFactory.CreateUpdateSocialLinksHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            VolunteerId.Create(volunteerId),
            Errors.General.NotFound(volunteerId));
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
        var command = new UpdateSocialLinksCommand()
        {
            Id = volunteer.Id.Id,
            SocialLinks = new List<SocialLinkDto>()
            {
                new("Title 1", "http://example.com"),
                new ("Title 2", "http://example2.com")
            }
        };

        var handler = StubFactory.CreateUpdateSocialLinksHandlerStub();

        // Act
        handler.VolunteersRepositoryMock.SetupGetById(
            volunteer.Id,
            volunteer);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handler.UnitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        volunteer.SocialLinksList.SocialLinks.Count.Should().Be(2);
    }
}