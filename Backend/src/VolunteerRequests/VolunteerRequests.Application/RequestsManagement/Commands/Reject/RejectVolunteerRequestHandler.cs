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
    private readonly IRequestsRepository _requestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectVolunteerRequestHandler> _logger;

    public RejectVolunteerRequestHandler(
        IRequestsRepository requestsRepository, 
        [FromKeyedServices(Constants.Context.VolunteerRequests)]IUnitOfWork unitOfWork,
        ILogger<RejectVolunteerRequestHandler> logger)
    {
        _requestsRepository = requestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> Handle(RejectVolunteerRequestCommand command, CancellationToken cancellationToken)
    {
        var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _requestsRepository.GetById(requestId, cancellationToken);

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