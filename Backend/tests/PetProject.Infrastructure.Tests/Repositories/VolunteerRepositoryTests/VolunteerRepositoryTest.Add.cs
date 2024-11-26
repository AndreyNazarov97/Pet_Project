using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetProject.SharedTestData;

namespace PetProject.Infrastructure.Tests.Repositories.VolunteerRepositoryTests;

public partial class VolunteerRepositoryTest
{
    [Fact]
    public async Task Add_ShouldAddVolunteerToDatabase()
    {
        var volunteer = TestData.Volunteer;

        var result = await _sut.Add(volunteer, CancellationToken.None);
        
        var addedVolunteer = await _sut.GetById(volunteer.Id, CancellationToken.None);
        
        await using var dbContext = _fixture.GetDbContext();
        var volunteersPhones = await dbContext.Volunteers
            .Where(v => v.Id == volunteer.Id)
            .Select(v => v.PhoneNumber.Value)
            .ToListAsync();

        result.Should().Be(volunteer.Id);
        addedVolunteer.IsSuccess.Should().BeTrue();
        volunteersPhones.Should()
            .HaveCount(1)
            .And.Contain(volunteer.PhoneNumber.Value);
    }
}