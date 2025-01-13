using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Application.AccountManagement.Commands.Login;
using PetProject.Accounts.Contracts.Responses;
using PetProject.SharedKernel.Shared;

namespace PetProject.IntegrationTests.AccountsManagement;

public class LoginHandlerTest : AccountsManagementTestsBase
{
    private readonly IRequestHandler<LoginUserCommand, Result<LoginResponse, ErrorList>> _sut;

    public LoginHandlerTest(AccountsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<LoginUserCommand, Result<LoginResponse, ErrorList>>>();
    }

    [Fact]
    public async Task ShouldReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new LoginUserCommand
        {
            Email = "ZVtZl@example.com",
            Password = "Test123@"
        };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public async Task ShouldReturnError_WhenPasswordIsInvalid()
    {
        // Arrange
        var password = "Test123@";
        var user = await SeedUser(password);

        var command = new LoginUserCommand
        {
            Email = user.Email!,
            Password = "InvalidPassword"
        };
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldLoginUserSuccessfully()
    {
        // Arrange
        var password = "Test123@";
        var user = await SeedUser(password);

        var command = new LoginUserCommand
        {
            Email = user.Email!,
            Password = password
        };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be(command.Email);
        result.Value.UserId.Should().Be(user.Id);
    }
}