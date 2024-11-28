using System.Data;
using Moq;
using PetProject.Core.Database;

namespace PetProject.Application.Tests.Extensions;

public static class UnitOfWorkExtensions
{
    public static void SetupTransaction(
        this Mock<IUnitOfWork> mock)
    {
        var transaction = new Mock<IDbTransaction>().Object;
        mock.Setup(uow => uow.BeginTransactionAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);
    }
}