using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialNetworks;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.AccountsManagement;

public class UpdateSocialNetworksHandlerTest : AccountsManagementTestsBase
{
    private readonly IRequestHandler<UpdateSocialNetworksCommand, Result<long, ErrorList>> _sut;

    public UpdateSocialNetworksHandlerTest(AccountsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<UpdateSocialNetworksCommand, Result<long, ErrorList>>>();
    }

    private static UpdateSocialNetworksCommand Command => new()
    {
        UserId = Random.Long,
        SocialLinks = new List<SocialNetworkDto>()
        {
            new(){Title = "Title 1", Url = "http://example1.com"},
            new(){Title = "Title 2", Url = "http://example2.com"},
        }
    };
    
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
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
    public async Task Handle_ShouldUpdateSocialLinks_WhenVolunteerExists()
    {
        // Arrange
        var volunteerAccount = await SeedVolunteerAccount();
        var command = Command with { UserId = volunteerAccount.UserId };
    
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
    
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var volunteerAccountFromDb = await _accountsDbContext.VolunteerAccounts
            .FirstOrDefaultAsync(x => x.Id == volunteerAccount.Id, CancellationToken.None);
         
        volunteerAccountFromDb.Should().NotBeNull();
        volunteerAccountFromDb!.User.SocialNetworks.Should().HaveCount(2);

        var socialNetwork1 = volunteerAccountFromDb.User.SocialNetworks
            .First(x => x.Title == command.SocialLinks.First().Title);
        socialNetwork1.Url.Should().Be(command.SocialLinks.First().Url);
        socialNetwork1.Title.Should().Be(command.SocialLinks.First().Title);
        
        var socialNetwork2 = volunteerAccountFromDb.User.SocialNetworks
            .First(x => x.Title == command.SocialLinks.Last().Title);
        socialNetwork2.Url.Should().Be(command.SocialLinks.Last().Url);
    }
}