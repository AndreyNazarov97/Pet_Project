using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.ObjectMappers;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.Aggregate;

namespace VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;

public class CreateVolunteerRequestHandler 
    : IRequestHandler<CreateVolunteerRequestCommand, Result<Guid, ErrorList>>
{
    private readonly IRequestsRepository _requestsRepository;
    private readonly ILogger<CreateVolunteerRequestHandler> _logger;

    public CreateVolunteerRequestHandler(
        IRequestsRepository requestsRepository,
        ILogger<CreateVolunteerRequestHandler> logger)
    {
        _requestsRepository = requestsRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(CreateVolunteerRequestCommand command, CancellationToken cancellationToken)
    {
        var requestResult = CreateVolunteerRequest(command);

        if (requestResult.IsFailure)
        {
            _logger.LogError("{Message}", requestResult.Error.Message);
            return requestResult.Error.ToErrorList();
        }
        
        var requestId = await _requestsRepository.Add(requestResult.Value, cancellationToken);

        return requestId;
    }

    private static Result<VolunteerRequest, Error> CreateVolunteerRequest(CreateVolunteerRequestCommand command)
    {
        var fullName = command.FullName.ToEntity();
        var description = Description.Create(command.Description).Value;
        var workExperience = Experience.Create(command.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var socialNetworks = command.SocialNetworksDto
            .Select(s => s.ToEntity());
        
        var volunteerInfo = new VolunteerInfo(fullName, phoneNumber, workExperience, description, socialNetworks);

        return VolunteerRequest.Create(volunteerInfo, command.UserId);
    }
}