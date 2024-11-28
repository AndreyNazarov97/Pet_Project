using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.SoftDeleteVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class SoftDeleteVolunteerHandlerStub : SoftDeleteVolunteerHandler
 {
     internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
     
     internal Mock<IUnitOfWork> UnitOfWorkMock { get; }
    
     internal Mock<ILogger<SoftDeleteVolunteerHandler>> LoggerMock { get; }
    
     public SoftDeleteVolunteerHandlerStub(
         Mock<IVolunteersRepository> volunteersRepositoryMock,
         Mock<IUnitOfWork> unitOfWorkMock,
         Mock<ILogger<SoftDeleteVolunteerHandler>> loggerMock) 
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