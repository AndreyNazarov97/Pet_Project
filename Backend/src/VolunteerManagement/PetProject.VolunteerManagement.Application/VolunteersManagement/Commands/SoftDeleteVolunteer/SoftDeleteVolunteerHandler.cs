using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeleteVolunteer;

public class SoftDeleteVolunteerHandler : IRequestHandler<SoftDeleteVolunteerCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;

    public SoftDeleteVolunteerHandler(
        IVolunteersRepository repository, 
        [FromKeyedServices(Constants.Context.VolunteerManagement)]IUnitOfWork unitOfWork,
        ILogger<SoftDeleteVolunteerHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(SoftDeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.Deactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.Log(LogLevel.Information, "Volunteer deleted with Id {volunteerId}", volunteerId);
        
        return UnitResult.Success<ErrorList>();
    }
}