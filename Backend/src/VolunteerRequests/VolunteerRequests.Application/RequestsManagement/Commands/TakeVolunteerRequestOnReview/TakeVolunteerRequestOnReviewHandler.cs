using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;

namespace VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

public class TakeVolunteerRequestOnReviewHandler : IRequestHandler< TakeVolunteerRequestOnReviewCommand, UnitResult<ErrorList>>
{
    private readonly IRequestsRepository _requestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TakeVolunteerRequestOnReviewHandler> _logger;

    public TakeVolunteerRequestOnReviewHandler(
        IRequestsRepository requestsRepository, 
        [FromKeyedServices(Constants.Context.VolunteerRequests)]IUnitOfWork unitOfWork,
        ILogger<TakeVolunteerRequestOnReviewHandler> logger)
    {
        _requestsRepository = requestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(TakeVolunteerRequestOnReviewCommand command, CancellationToken cancellationToken)
    {
        var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _requestsRepository.GetById(requestId, cancellationToken);

        if (requestResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerRequestId).ToErrorList();
        
        //TODO реализовать контракт для создания дискуссии
        var discussionId = DiscussionId.NewId();
        
        var request = requestResult.Value;
        var takeOnReviewResult = request.TakeOnReview(command.AdminId, discussionId);
        
        if (takeOnReviewResult.IsFailure)
            return takeOnReviewResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success<ErrorList>();
    }
}