using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.SetMainPetPhoto;

public class SetMainPetPhotoHandler : IRequestHandler<SetMainPetPhotoCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetMainPetPhotoHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(SetMainPetPhotoCommand command, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var volunteer = volunteerResult.Value;
        var pet = volunteer.GetById(PetId.Create(command.PetId));
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var extension = Path.GetExtension(command.Path);
        var path = Path.GetFileNameWithoutExtension(command.Path);
        var filePath = FilePath.Create(path, extension).Value;
        
        pet.SetMainPhoto(new PetPhoto(filePath));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return UnitResult.Success<ErrorList>();
    }
}