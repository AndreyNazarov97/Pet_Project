using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using PetProject.SharedKernel.Interfaces;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeleteVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class SoftDeleteVolunteerHandlerStub : SoftDeleteVolunteerHandler
 {
     internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
     
     internal Mock<IDateTimeProvider> DateTimeProviderMock { get; }
     internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
     internal Mock<ILogger<SoftDeleteVolunteerHandler>> LoggerMock { get; }
    
     public SoftDeleteVolunteerHandlerStub(
         Mock<IVolunteersRepository> volunteersRepositoryMock,
         Mock<IDateTimeProvider> dateTimeProviderMock,
         Mock<IUnitOfWork> unitOfWorkMock,
         Mock<ILogger<SoftDeleteVolunteerHandler>> loggerMock) 
         : base(
             volunteersRepositoryMock.Object,
             dateTimeProviderMock.Object,
             unitOfWorkMock.Object,
             loggerMock.Object)
     {
         VolunteersRepositoryMock = volunteersRepositoryMock;
         UnitOfWorkMock = unitOfWorkMock;
         LoggerMock = loggerMock;
     }
  
    
}