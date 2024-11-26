using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Application.Abstractions;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.VolunteersManagement;
using PetProject.Application.VolunteersManagement.AddPetPhoto;
using PetProject.Application.VolunteersManagement.CreateVolunteer;
using PetProject.Application.VolunteersManagement.SoftDeleteVolunteer;
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

    public static SoftDeleteVolunteerHandlerStub CreateDeleteVolunteerHandlerStub()
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<SoftDeleteVolunteerHandler>>());
    
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
    
    public static AddPetPhotoHandlerStub CreateAddPetPhotoHandlerStub() 
        => new(new Mock<IVolunteersRepository>(),
            new Mock<IFileProvider>(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<AddPetPhotoHandler>>());

    public static UpdatePetHandlerStub CreateUpdatePetHandlerStub()
        => new(new Mock<IVolunteersRepository>(),
            new Mock<ISpeciesRepository>(),
            new Mock<IUnitOfWork>());
    
    public static ChangePetStatusHandlerStub CreateChangePetStatusHandlerStub()
        => new(new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>());
    
    public static SoftDeletePetHandlerStub CreateSoftDeletePetHandlerStub()
        => new(new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>());

}
