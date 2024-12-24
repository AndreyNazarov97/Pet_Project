using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Interfaces;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeletePet;

public class SoftDeletePetHandler : IRequestHandler<SoftDeletePetCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public SoftDeletePetHandler(
        IVolunteersRepository volunteersRepository,
        IDateTimeProvider dateTimeProvider,
        [FromKeyedServices(Constants.Context.VolunteerManagement)]IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(SoftDeletePetCommand command, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.GetById(PetId.Create(command.PetId));
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();
        
        volunteerResult.Value.DeletePetSoft(pet.Id, _dateTimeProvider.UtcNow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return UnitResult.Success<ErrorList>();
    }
}