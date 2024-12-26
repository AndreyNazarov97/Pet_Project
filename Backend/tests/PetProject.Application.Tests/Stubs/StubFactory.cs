using Microsoft.Extensions.Logging;
using Moq;
using PetProject.Core.Database;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.Core.Messaging;
using PetProject.VolunteerManagement.Application.Providers;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.AddPetPhoto;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.CreateVolunteer;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeleteVolunteer;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.UpdateVolunteer;
using VolunteerRequests.Application.RequestsManagement.Commands.UpdateVolunteerRequest;

namespace PetProject.Application.Tests.Stubs;

public static class StubFactory
{
    public static GetVolunteerHandlerStub CreateGetVolunteerHandlerStub()
        => new(new Mock<IVolunteersRepository>());
    
    public static GetVolunteersListHandlerStub CreateGetVolunteersListHandlerStub() 
        => new(new Mock<IReadRepository>());
    
    public static CreateVolunteerHandlerStub CreateVolunteerHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IReadRepository>(),
            new Mock<ILogger<CreateVolunteerHandler>>());

    public static SoftDeleteVolunteerHandlerStub CreateDeleteVolunteerHandlerStub()
        => new(
            new Mock<IVolunteersRepository>(),
            new(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<SoftDeleteVolunteerHandler>>());
    
    public static UpdateVolunteerHandlerStub CreateUpdateVolunteerHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<UpdateVolunteerHandler>>());

    // public static UpdateRequisitesHandlerStub CreateUpdateRequisitesHandlerStub()
    //     => new(
    //         new UserManager<User>(),
    //         new Mock<IAccountManager>(),
    //         new Mock<IUnitOfWork>(),
    //         new Mock<ILogger<UpdateRequisitesHandler>>());
    //
    // public static UpdateSocialNetworksHandlerStub CreateUpdateSocialLinksHandlerStub() 
    //     => new(
    //         new UserManager<User>(),
    //         new Mock<IUnitOfWork>(),
    //         new Mock<ILogger<UpdateSocialNetworksHandler>>());
    
    public static CreatePetHandlerStub CreateCreatePetHandlerStub() 
        => new(
            new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>(),
            new Mock<IReadRepository>());
    
    public static AddPetPhotoHandlerStub CreateAddPetPhotoHandlerStub() 
        => new(new Mock<IVolunteersRepository>(),
            new Mock<IFileProvider>(),
            new Mock<IMessageQueue<IEnumerable<FileMetaDataDto>>>(),
            new Mock<IUnitOfWork>(),
            new Mock<ILogger<AddPetPhotoHandler>>());

    public static UpdatePetHandlerStub CreateUpdatePetHandlerStub()
        => new(new Mock<IVolunteersRepository>(),
            new Mock<IReadRepository>(),
            new Mock<IUnitOfWork>());
    
    public static ChangePetStatusHandlerStub CreateChangePetStatusHandlerStub()
        => new(new Mock<IVolunteersRepository>(),
            new Mock<IUnitOfWork>());
    
    public static SoftDeletePetHandlerStub CreateSoftDeletePetHandlerStub()
        => new(new Mock<IVolunteersRepository>(),
            new(),
            new Mock<IUnitOfWork>());

    public static CreateVolunteerRequestHandlerStub CreateCreateVolunteerRequestHandlerStub()
        => new(
            new(),
            new());

    public static TakeVolunteerRequestOnReviewHandlerStub CreateTakeVolunteerRequestOnReviewHandlerStub()
        => new(
            new(),
            new(),
            new());
    
    public static ApproveVolunteerRequestHandlerStub CreateApproveVolunteerRequestHandlerStub()
        => new(
            new(),
            new(),
            new());
    
    public static RejectVolunteerRequestHandlerStub CreateRejectVolunteerRequestHandlerStub()
        => new(
            new(),
            new(),
            new());
    
    public static SendForRevisionHandlerStub CreateSendForRevisionHandlerStub()
        => new(
            new(),
            new(),
            new());
    
    public static UpdateVolunteerRequestHandlerStub CreateUpdateVolunteerRequestHandlerStub()
        => new(
            new(),
            new(),
            new());
}
