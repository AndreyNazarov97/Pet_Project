using Microsoft.Extensions.DependencyInjection;
using PetProject.SharedTestData;
using PetProject.SpeciesManagement.Infrastructure.DbContexts;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;

namespace PetProject.IntegrationTests.VolunteerManagement;

public class VolunteerManagementTestsBase : IClassFixture<VolunteerManagementTestsWebFactory>, IAsyncLifetime
{
    protected readonly VolunteerManagementTestsWebFactory _factory;
    protected readonly IServiceScope _scope;
    protected readonly VolunteerDbContext _volunteerDbContext;
    private readonly SpeciesDbContext _speciesDbContext;

    public VolunteerManagementTestsBase(VolunteerManagementTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _volunteerDbContext = _scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
        _speciesDbContext = _scope.ServiceProvider.GetRequiredService<SpeciesDbContext>();
    }
    
    protected async Task<Volunteer> SeedVolunteer(CancellationToken cancellationToken = default)
    {
        var volunteer = TestData.Volunteer;
        var pet = TestData.Pet;
        volunteer.AddPet(pet);
        
        await _volunteerDbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        
        await _volunteerDbContext.SaveChangesAsync(cancellationToken);

        return volunteer;
    }
    
    protected async Task SeedSpecies(CancellationToken cancellationToken = default)
    {
        var species = TestData.Species;
        
        await _speciesDbContext.Species.AddAsync(species, cancellationToken);
        await _speciesDbContext.SaveChangesAsync(cancellationToken);
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