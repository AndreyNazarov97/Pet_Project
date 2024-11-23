using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.CreateVolunteer;
using PetProject.Application.VolunteersManagement.DeleteVolunteer;
using PetProject.Application.VolunteersManagement.UpdateRequisites;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;
using PetProject.Application.VolunteersManagement.UpdateVolunteer;

namespace PetProject.Application.Tests.Stubs;

public static class StubFactory
{
    public static GetVolunteerHandlerStub CreateGetVolunteerHandlerStub()
        => new(new Mock<IVolunteersRepository>());
    
    public static GetVolunteersListHandlerStub CreateGetVolunteersListHandlerStub() 
        => new(new Mock<IVolunteersRepository>());
    
    public static CreateVolunteerHandlerStub CreateVolunteerHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<ILogger<CreateVolunteerHandler>>());
    
    public static DeleteVolunteerHandlerStub CreateDeleteVolunteerHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<ILogger<DeleteVolunteerHandler>>());
    
    public static UpdateVolunteerHandlerStub CreateUpdateVolunteerHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<UpdateVolunteerHandler>>());
    
    public static UpdateRequisitesHandlerStub CreateUpdateRequisitesHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<UpdateRequisitesHandler>>());
    
    public static UpdateSocialLinksHandlerStub CreateUpdateSocialLinksHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<UpdateSocialLinksHandler>>());
    
    public static CreatePetHandlerStub CreateCreatePetHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>(),
            new Mock<ISpeciesRepository>());
    
}
