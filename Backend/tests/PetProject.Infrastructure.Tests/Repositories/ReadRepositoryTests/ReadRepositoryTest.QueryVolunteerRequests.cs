using FluentAssertions;
using PetProject.Core.Database.Models;
using PetProject.SharedTestData;
using VolunteerRequests.Domain.Aggregate;
using VolunteerRequests.Domain.Enums;
using VolunteerRequests.Domain.ValueObjects;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.Infrastructure.Tests.Repositories.ReadRepositoryTests;

public partial class ReadRepositoryTest
{
    [Fact]
    public async Task ShouldReturnVolunteerRequestWithGivenId()
    {
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var adminId = Random.Long;
        var discussionId = Guid.NewGuid();
        request.TakeOnReview(adminId, discussionId);
        request.SendForRevision(RejectionComment.Create(Random.Words).Value);
        
        await using var dbContext = _fixture.GetVolunteerRequestsDbContext();
        await dbContext.AddAsync(request);
        await dbContext.SaveChangesAsync();

        var query = new VolunteerRequestQueryModel()
        {
            VolunteerRequestIds = [request.Id]
        };
        
        var result = await _sut.QueryVolunteerRequests(query, CancellationToken.None);
        
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(request.Id);
        result[0].VolunteerInfo.FullName.Name.Should().Be(request.VolunteerInfo.FullName.Name);
        result[0].VolunteerInfo.FullName.Surname.Should().Be(request.VolunteerInfo.FullName.Surname);
        result[0].VolunteerInfo.FullName.Patronymic.Should().Be(request.VolunteerInfo.FullName.Patronymic);
        result[0].VolunteerInfo.GeneralDescription.Should().Be(request.VolunteerInfo.GeneralDescription.Value);
        result[0].VolunteerInfo.WorkExperience.Should().Be(request.VolunteerInfo.WorkExperience.Years);
        result[0].VolunteerInfo.PhoneNumber.Should().Be(request.VolunteerInfo.PhoneNumber.Value);
        result[0].VolunteerInfo.SocialNetworks.Length.Should().Be(request.VolunteerInfo.SocialNetworks.Count);
        result[0].RequestStatus.Should().Be(request.RequestStatus.ToString());
        result[0].CreatedAt.Should().BeCloseTo(request.CreatedAt, TimeSpan.FromMilliseconds(10));
        result[0].UserId.Should().Be(request.UserId);
        result[0].AdminId.Should().Be(adminId);
        result[0].DiscussionId.Should().Be(discussionId);
        result[0].RejectionComment.Should().Be(request.RejectionComment.Value);
    }
    
    [Fact]
    public async Task ShouldReturnEmptyListWhenNoVolunteerRequests()
    {
        var query = new VolunteerRequestQueryModel()
        {
            VolunteerRequestIds = []
        };
        
        var result = await _sut.QueryVolunteerRequests(query, CancellationToken.None);
        
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnAllVolunteerRequests_WhenNoFiltersAreApplied()
    {
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var request2 = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        await using var dbContext = _fixture.GetVolunteerRequestsDbContext();
        await dbContext.AddAsync(request);
        await dbContext.AddAsync(request2);
        await dbContext.SaveChangesAsync();
        
        var query = new VolunteerRequestQueryModel();
        
        var result = await _sut.QueryVolunteerRequests(query, CancellationToken.None);
        
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteerRequestsByAdminId()
    {
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var adminId = Random.Long;
        var request2 = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        request2.TakeOnReview(adminId, Guid.NewGuid());
        await using var dbContext = _fixture.GetVolunteerRequestsDbContext();
        await dbContext.AddAsync(request);
        await dbContext.AddAsync(request2);
        await dbContext.SaveChangesAsync();
        
        var query = new VolunteerRequestQueryModel()
        {
            AdminIds = [adminId]
        };
        
        var result = await _sut.QueryVolunteerRequests(query, CancellationToken.None);
        
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task ShouldReturnVolunteerRequestsByUserId()
    {
        var userId = Random.Long;
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, userId).Value;
        await using var dbContext = _fixture.GetVolunteerRequestsDbContext();
        await dbContext.AddAsync(request);
        await dbContext.SaveChangesAsync();

        var query = new VolunteerRequestQueryModel()
        {
            UserIds = [userId]
        };

        var result = await _sut.QueryVolunteerRequests(query, CancellationToken.None);

        result.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task ShouldReturnVolunteerRequestsByStatus()
    {
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        var request2 = VolunteerRequest.Create(TestData.VolunteerInfo, Random.Long).Value;
        request2.SendForRevision(RejectionComment.Create(Random.Words).Value);
        await using var dbContext = _fixture.GetVolunteerRequestsDbContext();
        await dbContext.AddAsync(request);
        await dbContext.AddAsync(request2);
        await dbContext.SaveChangesAsync();
        
        var query = new VolunteerRequestQueryModel()
        {
            RequestStatus = RequestStatus.RevisionRequired.ToString()
        };
        
        var result = await _sut.QueryVolunteerRequests(query, CancellationToken.None);
        
        result.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task ShouldSortVolunteerRequestsByUserId()
    {
        var userId1 = 1L;
        var userId2 = 2L;
        var request = VolunteerRequest.Create(TestData.VolunteerInfo, userId1).Value;
        var request2 = VolunteerRequest.Create(TestData.VolunteerInfo, userId2).Value;
        var request3 = VolunteerRequest.Create(TestData.VolunteerInfo, userId1).Value;
        
        await using var dbContext = _fixture.GetVolunteerRequestsDbContext();
        await dbContext.AddAsync(request);
        await dbContext.AddAsync(request2);
        await dbContext.AddAsync(request3);
        await dbContext.SaveChangesAsync();
        
        var query = new VolunteerRequestQueryModel()
        {
            SortBy = "UserId"
        };
        
        var result = await _sut.QueryVolunteerRequests(query, CancellationToken.None);
        
        result[0].UserId.Should().Be(request.UserId);
        result[1].UserId.Should().Be(request3.UserId);
        result[2].UserId.Should().Be(request2.UserId);
    }
}