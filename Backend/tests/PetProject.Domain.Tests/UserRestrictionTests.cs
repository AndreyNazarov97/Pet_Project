using FluentAssertions;
using VolunteerRequests.Domain.Aggregate;

namespace PetProject.Domain.Tests;

public class UserRestrictionTests
{
    [Fact]
    public void Create_ShouldCreateUserRestriction()
    {
        // Arrange
        var userId = 123L;

        // Act
        var result = UserRestriction.Create(userId);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be(userId);
    }
    
    [Fact]
    public void Create_ShouldFailWhenUserIdIsNegative()
    {
        // Arrange
        var userId = -123L;

        // Act
        var result = UserRestriction.Create(userId);
        
        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public void CheckExpirationOfBan_ShouldFailWhenBanIsNotExpired()
    {
        // Arrange
        var ban = UserRestriction.Create(123L).Value;
        
        // Act
        var result = ban.CheckExpirationOfBan();
        
        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public void RemoveBan_ShouldRemoveBan()
    {
        // Arrange
        var restriction = UserRestriction.Create(123L).Value;
        
        // Act
        var result = restriction.RemoveBan();
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        restriction.CheckExpirationOfBan().IsSuccess.Should().BeTrue();
    }
}