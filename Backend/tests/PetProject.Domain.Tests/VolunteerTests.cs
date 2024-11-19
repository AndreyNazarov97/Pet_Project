using FluentAssertions;
using PetProject.Domain.Shared;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Domain.Tests;

public class VolunteerTests
{
    private readonly Volunteer _volunteer;
    private readonly Pet _firstPet;

    public VolunteerTests()
    {
        _volunteer = TestData.Volunteer;

        _firstPet = TestData.Pet;
    }

    [Fact]
    public void AddPet_ShouldAssignCorrectPosition()
    {
        // Act
        _volunteer.AddPet(_firstPet);

        // Assert
        _firstPet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void AddMultiplePets_ShouldAssignSequentialPositions()
    {
        // Arrange
        var secondPet = TestData.Pet;
        var thirdPet = TestData.Pet;

        // Act
        _volunteer.AddPet(_firstPet);
        _volunteer.AddPet(secondPet);
        _volunteer.AddPet(thirdPet);

        // Assert
        _volunteer.Pets!.Select(p => p.Position.Value).Should()
            .BeEquivalentTo(new[] { 1, 2, 3 });
    }
    
    [Fact]
    public void ChangePetPosition_MovesPetToStart()
    {
        // Arrange
        var secondPet = TestData.Pet;
        var thirdPet = TestData.Pet;
        _volunteer.AddPet(_firstPet);
        _volunteer.AddPet(secondPet);
        _volunteer.AddPet(thirdPet);

        // Act
        var result = _volunteer.ChangePetPosition(thirdPet, Position.Create(1).Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _volunteer.Pets!.Select(p => p.Position.Value)
            .Should().BeEquivalentTo(new[] { 2, 3, 1 });
    }
    
    [Fact]
    public void RemovePet_ShouldReassignPositionsCorrectly()
    {
        // Arrange
        var secondPet = TestData.Pet;
        var thirdPet = TestData.Pet;
        _volunteer.AddPet(_firstPet);
        _volunteer.AddPet(secondPet);
        _volunteer.AddPet(thirdPet);
            
        // Act
        _volunteer.RemovePet(_firstPet);

        // Assert
        _volunteer.Pets!.Select(p => p.Position.Value)
            .Should().BeEquivalentTo(new[] { 1, 2 });
    }

    [Fact]
    public void ChangePetPosition_ReturnsError_WhenPetNotFound()
    {
        //Act
        var result = _volunteer.ChangePetPosition(_firstPet, Position.Create(1).Value);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public void ChangePetPosition_ReturnsError_WhenPetPositionIsGreaterThanPetsCount()
    {
        //Arrange
        _volunteer.AddPet(_firstPet);

        //Act
        var result = _volunteer.ChangePetPosition(_firstPet, Position.Create(2).Value);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public void ChangePetPosition_DoesNotChangePositionIfAlreadyAtDesiredPosition()
    {
        // Arrange
        _volunteer.AddPet(_firstPet);
            
        // Act
        var result = _volunteer.ChangePetPosition(_firstPet, Position.Create(1).Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _firstPet.Position.Value.Should().Be(1);
    }
    
    [Fact]
    public void ChangePetPosition_ShouldNotAllowDuplicatePetPositions()
    {
        //Arrange
        var secondPet = TestData.Pet;
        var thirdPet = TestData.Pet;
        var fourthPet = TestData.Pet;
        _volunteer.AddPet(_firstPet);
        _volunteer.AddPet(secondPet);
        _volunteer.AddPet(thirdPet);
        _volunteer.AddPet(fourthPet);

        //Act
        var result = _volunteer.ChangePetPosition(_firstPet, Position.Create(3).Value);

        //Assert
        result.IsSuccess.Should().BeTrue();
        var positions = _volunteer.Pets!.Select(p => p.Position.Value).ToList();
        positions.Should().OnlyHaveUniqueItems("у всех животных должны быть уникальные позиции");
        positions.Should().BeEquivalentTo(new[] { 3, 1, 2, 4 }, "позиции должны корректно обновиться");
    }
}