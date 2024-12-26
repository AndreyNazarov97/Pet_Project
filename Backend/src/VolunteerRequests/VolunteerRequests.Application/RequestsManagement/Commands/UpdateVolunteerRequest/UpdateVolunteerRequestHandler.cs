using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.Core.ObjectMappers;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

namespace VolunteerRequests.Application.RequestsManagement.Commands.UpdateVolunteerRequest;

public class UpdateVolunteerRequestHandler : IRequestHandler<UpdateVolunteerRequestCommand, UnitResult<ErrorList>>
{
    private readonly IRequestsRepository _requestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVolunteerRequestHandler> _logger;

    public UpdateVolunteerRequestHandler(
        IRequestsRepository requestsRepository,
        [FromKeyedServices(Constants.Context.VolunteerRequests)]
        IUnitOfWork unitOfWork,
        ILogger<UpdateVolunteerRequestHandler> logger)
    {
        _requestsRepository = requestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(UpdateVolunteerRequestCommand command,
        CancellationToken cancellationToken)
    {
        var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _requestsRepository.GetById(requestId, cancellationToken);

        if (requestResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerRequestId).ToErrorList();

        var request = requestResult.Value;

        if (command.UserId != request.UserId)
            return Errors.VolunteerRequests.AccessDenied().ToErrorList();

        var volunteerInfo = CreateInfo(command);
        
        var updateResult = request.UpdateInfo(volunteerInfo);

        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<ErrorList>();
    }

    private static VolunteerInfo CreateInfo(UpdateVolunteerRequestCommand command)
    {
        var fullName = command.FullName.ToEntity();
        var description = Description.Create(command.Description).Value;
        var workExperience = Experience.Create(command.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var socialNetworks = command.SocialNetworksDto
            .Select(s => s.ToEntity());

        var volunteerInfo = new VolunteerInfo(fullName, phoneNumber, workExperience, description, socialNetworks);

        return volunteerInfo;
    }
}