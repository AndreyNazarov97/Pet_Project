using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using PetProject.Accounts.Contracts;
using PetProject.Accounts.Infrastructure.DataSeed;
using PetProject.Core.Dtos.Accounts;
using PetProject.SharedKernel.Shared;
using PetProject.SpeciesManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;

namespace PetProject.IntegrationTests.DiscussionsManagement;

public class DiscussionsTestsWebFactory : BaseTestsWebFactory
{
    private readonly IAccountsContract _accountsContractMock =
        Substitute.For<IAccountsContract>();
    
    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);
        services.RemoveAll(typeof(AccountsSeeder));
        services.RemoveAll(typeof(VolunteersSeeder));
        services.RemoveAll(typeof(SpeciesSeeder));
        
        services.RemoveAll(typeof(IAccountsContract));
        
        services.AddScoped<IAccountsContract>(_ => _accountsContractMock);
    }

    public void SetupGetUserById(long userId , Result<UserDto, ErrorList> result)
    {
        _accountsContractMock
            .GetUserById(userId, Arg.Any<CancellationToken>())
            .Returns(result);
    }
}