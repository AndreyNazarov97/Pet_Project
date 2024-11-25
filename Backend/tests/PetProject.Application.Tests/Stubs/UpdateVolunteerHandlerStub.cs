﻿using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;
using PetProject.Application.VolunteersManagement.UpdateVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class UpdateVolunteerHandlerStub : UpdateVolunteerHandler
{
    internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
    internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
    internal Mock<ILogger<UpdateVolunteerHandler>> LoggerMock { get; }
    
    public UpdateVolunteerHandlerStub(
        Mock<IVolunteersRepository> volunteersRepositoryMock,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<ILogger<UpdateVolunteerHandler>> loggerMock) 
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