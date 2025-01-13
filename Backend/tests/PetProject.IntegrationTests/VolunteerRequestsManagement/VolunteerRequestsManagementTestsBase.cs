using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Infrastructure.DbContexts;
using VolunteerRequests.Domain.Aggregate;
using VolunteerRequests.Infrastructure.DbContexts;

namespace PetProject.IntegrationTests.VolunteerRequestsManagement;

public class VolunteerRequestsManagementTestsBase : IClassFixture<VolunteerRequestsTestsWebFactory>, IAsyncLifetime
{
    protected readonly VolunteerRequestsTestsWebFactory _factory;
    protected readonly IServiceScope _scope;
    protected readonly VolunteerRequestsDbContext _volunteerRequestsDbContext;

    public VolunteerRequestsManagementTestsBase(VolunteerRequestsTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _volunteerRequestsDbContext = _scope.ServiceProvider.GetRequiredService<VolunteerRequestsDbContext>();
    }
    
    protected async Task SeedVolunteerRequest(VolunteerRequest volunteerRequest ,CancellationToken cancellationToken = default)
    {
        await _volunteerRequestsDbContext.VolunteerRequests.AddAsync(volunteerRequest, cancellationToken);
        
        await _volunteerRequestsDbContext.SaveChangesAsync(cancellationToken);
    }
    
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
        
        _scope.Dispose();
    }
}