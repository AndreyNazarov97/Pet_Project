using CSharpFunctionalExtensions;
using Moq;
using PetProject.Accounts.Contracts;
using PetProject.Core.Dtos.Accounts;
using PetProject.SharedKernel.Shared;

namespace PetProject.Application.Tests.Extensions;

public static class AccountsContractExtension
{
    public static void SetupGetById(this Mock<IAccountsContract> mock, Result<UserDto, ErrorList> result)
    {
        mock.Setup(x => x.GetUserById(
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    }
}