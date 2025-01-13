using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Application.RequestsManagement.Commands.Reject;

public class RejectVolunteerRequestHandler : IRequestHandler<RejectVolunteerRequestCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly ILogger<RejectVolunteerRequestHandler> _logger;

    public RejectVolunteerRequestHandler(
        IVolunteerRequestsRepository volunteerRequestsRepository, 
        [FromKeyedServices(Constants.Context.VolunteerRequests)]IUnitOfWork unitOfWork,
        IPublisher publisher,
        ILogger<RejectVolunteerRequestHandler> logger)
    {
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> Handle(RejectVolunteerRequestCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
            var requestResult = await _volunteerRequestsRepository.GetById(requestId, cancellationToken);

            if (requestResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerRequestId).ToErrorList();

            var request = requestResult.Value;

            if (command.AdminId != request.AdminId)
                return Errors.VolunteerRequests.AccessDenied().ToErrorList();
        
            var rejectResult = request.Reject(RejectionComment.Create(command.RejectionComment).Value);

            await _publisher.PublishDomainEvents(request, cancellationToken);
        
            if (rejectResult.IsFailure)
                return rejectResult.Error.ToErrorList();
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("Request {requestId} rejected", command.VolunteerRequestId);
        
            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            transaction.Rollback();

            _logger.LogError(e, "Error while rejecting request {requestId}", command.VolunteerRequestId);
            return Error.Failure("volunteer.request.reject", "Error while rejecting request").ToErrorList();
        }
        
      
    }
}