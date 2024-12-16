using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.ChangePetStatus;

public class ChangePetStatusHandler : IRequestHandler<ChangePetStatusCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePetStatusHandler(
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(Constants.Context.VolunteerManagement)]IUnitOfWork unitOfWork)
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

        var petId = PetId.Create(command.PetId);
        
        var result = volunteerResult.Value.ChangePetStatus(petId, command.HelpStatus);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return UnitResult.Success<ErrorList>();
    }
}