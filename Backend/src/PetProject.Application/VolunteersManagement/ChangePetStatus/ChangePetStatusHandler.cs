using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Abstractions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.VolunteersManagement.ChangePetStatus;

public class ChangePetStatusHandler : IRequestHandler<ChangePetStatusCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePetStatusHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(ChangePetStatusCommand command, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.GetById(PetId.Create(command.PetId));
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();
        
        pet.ChangeStatus(command.HelpStatus);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return UnitResult.Success<ErrorList>();
    }
}