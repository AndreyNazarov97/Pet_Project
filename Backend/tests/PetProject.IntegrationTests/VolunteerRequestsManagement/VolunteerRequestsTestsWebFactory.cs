using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using PetProject.Accounts.Infrastructure.DataSeed;
using PetProject.Discussions.Contracts;
using PetProject.Discussions.Contracts.Requests;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;
using PetProject.SpeciesManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;

namespace PetProject.IntegrationTests.VolunteerRequestsManagement;

public class VolunteerRequestsTestsWebFactory : BaseTestsWebFactory
{
    private readonly IDiscussionContract _discussionContract =
        Substitute.For<IDiscussionContract>();
    
    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);
        services.RemoveAll(typeof(AccountsSeeder));
        services.RemoveAll(typeof(VolunteersSeeder));
        services.RemoveAll(typeof(SpeciesSeeder));
        
        services.RemoveAll(typeof(IDiscussionContract));
        
        services.AddScoped<IDiscussionContract>(_ => _discussionContract);
    }

    public void SetupCreate( Result<Discussion, ErrorList> result)
    {
        _discussionContract
            .Create(Arg.Any<CreateDiscussionRequest>(), Arg.Any<CancellationToken>())
            .Returns(result);
    }
}