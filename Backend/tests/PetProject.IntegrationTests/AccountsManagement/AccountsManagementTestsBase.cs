using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Domain;
using PetProject.Accounts.Infrastructure.DbContexts;
using PetProject.SharedTestData;

namespace PetProject.IntegrationTests.AccountsManagement;

public class AccountsManagementTestsBase : IClassFixture<AccountsTestsWebFactory>, IAsyncLifetime
{
    protected readonly AccountsTestsWebFactory _factory;
    protected readonly IServiceScope _scope;
    protected readonly AccountsDbContext _accountsDbContext;
    private readonly UserManager<User> _userManager;

    public AccountsManagementTestsBase(AccountsTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _accountsDbContext = _scope.ServiceProvider.GetRequiredService<AccountsDbContext>();
        _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    }

    protected async Task<VolunteerAccount> SeedVolunteerAccount(CancellationToken cancellationToken = default)
    {
        var volunteerAccount = TestData.VolunteerAccount;

        await _accountsDbContext.VolunteerAccounts.AddAsync(volunteerAccount, cancellationToken);

        await _accountsDbContext.SaveChangesAsync(cancellationToken);

        return volunteerAccount;
    }

    protected async Task<User> SeedUser(string password, CancellationToken cancellationToken = default)
    {
        var user = TestData.User;

        var result = await _userManager.CreateAsync(user, password);

        return user;
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