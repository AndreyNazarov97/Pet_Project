using FluentAssertions;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.VolunteerRepositoryTests;

public partial class VolunteerRepositoryTest
{
    [Fact]
    public async Task ShouldReturnError_WhenVolunteerNotExists()
    {
        var volunteer = TestData.Volunteer;
        var result = await _sut.GetById(volunteer.Id, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("record.not.found");
    }
    
    [Fact]
    public async Task ShouldReturnVolunteerWithGivenId()
    {
        var volunteer = TestData.Volunteer;
        await _sut.Add(volunteer, CancellationToken.None);
        
        var result = await _sut.GetById(volunteer.Id, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.PhoneNumber.Should().Be(volunteer.PhoneNumber);
    }
    
}