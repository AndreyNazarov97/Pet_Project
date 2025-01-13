using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Application.AccountManagement.Commands.Register;
using PetProject.SharedKernel.Shared;
using PetProject.SharedTestData.Creators;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.AccountsManagement;

public class RegisterHandlerTest : AccountsManagementTestsBase
{
    private readonly IRequestHandler<RegisterCommand, UnitResult<ErrorList>> _sut;

    public RegisterHandlerTest(AccountsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<RegisterCommand, UnitResult<ErrorList>>>();
    }
    
    private static RegisterCommand Command => new()
    {
        Email = Random.Email,
        UserName = Random.UserName,
        Password = "Test123@",
        FullName = DtoCreator.CreateFullNameDto()
    };
    
    [Fact]
    public async Task ShouldRegisterSuccessfully()
    {
        // Arrange
        var command = Command;
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var userFromDb = await _accountsDbContext.Users
            .FirstOrDefaultAsync(x => x.Email == command.Email, CancellationToken.None);
        
        userFromDb.Should().NotBeNull();
        userFromDb!.Email.Should().Be(command.Email);
        userFromDb.UserName.Should().Be(command.UserName);
        userFromDb.FullName.Name.Should().Be(command.FullName.Name);
        userFromDb.FullName.Surname.Should().Be(command.FullName.Surname);
        userFromDb.FullName.Patronymic.Should().Be(command.FullName.Patronymic);
    }
    
}