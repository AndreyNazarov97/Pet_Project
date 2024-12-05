using FluentAssertions;
using Moq;
using PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialNetworks;
using PetProject.Application.Tests.Stubs;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedTestData;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Application.Tests.AccountManagement;

public class UpdateSocialNetworksHandlerTest
{
    // TODO: Исправить тесты. Пока не понятно как мокать UserManager
    // private static UpdateSocialNetworksCommand Command => new()
    // {
    //     UserId = Random.Long,
    //     SocialLinks = new List<SocialNetworkDto>()
    //     {
    //         new(){Title = "Title 1", Url = "http://example1.com"},
    //         new(){Title = "Title 2", Url = "http://example2.com"},
    //     }
    // };
    //
    //
    // [Fact]
    // public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
    // {
    //     // Arrange
    //     var command = Command;
    //     var handler = StubFactory.CreateUpdateSocialLinksHandlerStub();
    //
    //     // Act
    //     handler.VolunteersRepositoryMock.SetupGetById(
    //         VolunteerId.Create(command.VolunteerId),
    //         Errors.General.NotFound(command.VolunteerId));
    //     var result = await handler.Handle(command, CancellationToken.None);
    //
    //     // Assert
    //     result.IsFailure.Should().BeTrue();
    //     result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
    //     handler.VolunteersRepositoryMock.Verify(repo => repo.GetById(
    //             It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()),
    //         Times.Once);
    // }
    //
    // [Fact]
    // public async Task Handle_ShouldUpdateSocialLinks_WhenVolunteerExists()
    // {
    //     // Arrange
    //     var volunteer = TestData.Volunteer;
    //     var command = Command with { VolunteerId = volunteer.Id.Id };
    //
    //     var handler = StubFactory.CreateUpdateSocialLinksHandlerStub();
    //
    //     // Act
    //     handler.VolunteersRepositoryMock.SetupGetById(
    //         volunteer.Id,
    //         volunteer);
    //     var result = await handler.Handle(command, CancellationToken.None);
    //
    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    //     handler.UnitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    //
    //     // volunteer.SocialLinks.Count.Should().Be(2);
    // }
}