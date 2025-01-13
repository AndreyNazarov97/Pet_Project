using Microsoft.Extensions.DependencyInjection;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Infrastructure.DbContexts;

namespace PetProject.IntegrationTests.DiscussionsManagement;

public class DiscussionsManagementTestBase: IClassFixture<DiscussionsTestsWebFactory>, IAsyncLifetime
{
    protected readonly DiscussionsTestsWebFactory _factory;
    protected readonly IServiceScope _scope;
    protected readonly DiscussionsDbContext _discussionsDbContext;

    public DiscussionsManagementTestBase(DiscussionsTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _discussionsDbContext = _scope.ServiceProvider.GetRequiredService<DiscussionsDbContext>();
    }
    
    protected async Task SeedDiscussion(Discussion discussion ,CancellationToken cancellationToken = default)
    {
        await _discussionsDbContext.Discussions.AddAsync(discussion, cancellationToken);
        
        await _discussionsDbContext.SaveChangesAsync(cancellationToken);
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