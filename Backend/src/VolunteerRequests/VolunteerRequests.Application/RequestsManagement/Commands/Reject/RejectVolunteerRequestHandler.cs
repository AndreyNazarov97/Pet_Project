using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Application.RequestsManagement.Commands.Reject;

public class RejectVolunteerRequestHandler : IRequestHandler<RejectVolunteerRequestCommand, UnitResult<ErrorList>>
{
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectVolunteerRequestHandler> _logger;

    public RejectVolunteerRequestHandler(
        IVolunteerRequestsRepository volunteerRequestsRepository, 
        [FromKeyedServices(Constants.Context.VolunteerRequests)]IUnitOfWork unitOfWork,
        ILogger<RejectVolunteerRequestHandler> logger)
    {
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> Handle(RejectVolunteerRequestCommand command, CancellationToken cancellationToken)
    {
        var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _volunteerRequestsRepository.GetById(requestId, cancellationToken);

        if (requestResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerRequestId).ToErrorList();

        var request = requestResult.Value;

        if (command.AdminId != request.AdminId)
            return Errors.VolunteerRequests.AccessDenied().ToErrorList();
        
        var rejectResult = request.Reject(RejectionComment.Create(command.RejectionComment).Value);
        
        if (rejectResult.IsFailure)
            return rejectResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success<ErrorList>();
    }
}