using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetProject.Accounts.Infrastructure.DataSeed;
using PetProject.Accounts.Infrastructure.Options;
using PetProject.SpeciesManagement.Infrastructure.DataSeed;
using PetProject.VolunteerManagement.Infrastructure.DataSeed;

namespace PetProject.IntegrationTests.AccountsManagement;

public class AccountsTestsWebFactory : BaseTestsWebFactory
{
    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);
        services.RemoveAll(typeof(AccountsSeeder));
        services.RemoveAll(typeof(VolunteersSeeder));
        services.RemoveAll(typeof(SpeciesSeeder));
        
        services.Configure<AdminOptions>(options =>
        {
            options.UserName = "TestAdmin";
            options.Email = "Test@mail.com";
            options.Password = "Test123@";
        });
        
            
        Environment.SetEnvironmentVariable("ACCOUNTS_JSON_PATH", "etc/accounts.json");
        
        services.AddSingleton<AccountsSeeder>();
    }
}