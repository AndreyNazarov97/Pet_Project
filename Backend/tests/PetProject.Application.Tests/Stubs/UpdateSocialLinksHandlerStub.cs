﻿using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;

namespace PetProject.Application.Tests.Stubs;

public class UpdateSocialLinksHandlerStub : UpdateSocialLinksHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<UpdateSocialLinksHandler>> LoggerMock { get; }
    
    public UpdateSocialLinksHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<UpdateSocialLinksHandler>> loggerMock) 
        : base(
            volunteersRepositoryMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object)
    {
        VolunteersRepositoryMock = volunteersRepositoryMock;
        UnitOfWorkMock = unitOfWorkMock;
        LoggerMock = loggerMock;
    }
}