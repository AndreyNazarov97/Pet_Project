using System.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Moq;
using PetProject.Application.Abstractions;

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