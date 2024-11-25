using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.CreateVolunteer;
using PetProject.Application.VolunteersManagement.DeleteVolunteer;

namespace PetProject.Application.Tests.Stubs;

public class DeleteVolunteerHandlerStub : DeleteVolunteerHandler
 {
     internal Mock<IVolunteersRepository> VolunteersRepositoryMock { get; }
    
     internal Mock<ILogger<DeleteVolunteerHandler>> LoggerMock { get; }
    
     public DeleteVolunteerHandlerStub(
         Mock<IVolunteersRepository> volunteersRepositoryMock,
         Mock<ILogger<DeleteVolunteerHandler>> loggerMock) 
         : base(
             volunteersRepositoryMock.Object,
             loggerMock.Object)
     {
         VolunteersRepositoryMock = volunteersRepositoryMock;
         LoggerMock = loggerMock;
     }
  
    
}