using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.RequestsManagement.Commands.Approve;

public class ApproveVolunteerRequestHandler : IRequestHandler<ApproveVolunteerRequestCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveVolunteerRequestHandler> _logger;

    public ApproveVolunteerRequestHandler(
        IVolunteerRequestsRepository volunteerRequestsRepository, 
        IPublisher publisher,
        [FromKeyedServices(Constants.Context.VolunteerRequests)]IUnitOfWork unitOfWork,
        ILogger<ApproveVolunteerRequestHandler> logger)
    {
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(ApproveVolunteerRequestCommand command, CancellationToken cancellationToken)
    {
        var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _volunteerRequestsRepository.GetById(requestId, cancellationToken);

        if (requestResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerRequestId).ToErrorList();

        var request = requestResult.Value;

        if (command.AdminId != request.AdminId)
            return Errors.VolunteerRequests.AccessDenied().ToErrorList();
        
        var approveResult = request.Approve();
        
        if (approveResult.IsFailure)
            return approveResult.Error.ToErrorList();
        
        var domainEvent = new VolunteerRequestApprovedEvent
        {
            VolunteerRequestId = requestResult.Value.Id,
            UserId = requestResult.Value.UserId
        };
        await _publisher.PublishDomainEvent(domainEvent, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success<ErrorList>();
    }
}