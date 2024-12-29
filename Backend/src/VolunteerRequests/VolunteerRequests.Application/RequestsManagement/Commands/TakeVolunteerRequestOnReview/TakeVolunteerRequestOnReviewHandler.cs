using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.Discussions.Contracts;
using PetProject.Discussions.Contracts.Requests;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using VolunteerRequests.Application.Repositories;

namespace VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

public class
    TakeVolunteerRequestOnReviewHandler : IRequestHandler<TakeVolunteerRequestOnReviewCommand,
    Result<DiscussionId, ErrorList>>
{
    private readonly IDiscussionContract _discussionContract;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TakeVolunteerRequestOnReviewHandler> _logger;

    public TakeVolunteerRequestOnReviewHandler(
        IDiscussionContract discussionContract,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        [FromKeyedServices(Constants.Context.VolunteerRequests)]
        IUnitOfWork unitOfWork,
        ILogger<TakeVolunteerRequestOnReviewHandler> logger)
    {
        _discussionContract = discussionContract;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<DiscussionId, ErrorList>> Handle(TakeVolunteerRequestOnReviewCommand command,
        CancellationToken cancellationToken)
    {
        var requestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var requestResult = await _volunteerRequestsRepository.GetById(requestId, cancellationToken);

        if (requestResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerRequestId).ToErrorList();

        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var createDiscussionRequest =
                new CreateDiscussionRequest(requestId.Id, command.AdminId, requestResult.Value.UserId);
            var discussionResult = await _discussionContract.Create(createDiscussionRequest, cancellationToken);
            if (discussionResult.IsFailure)
                return discussionResult.Error;

            var discussionId = discussionResult.Value.Id;

            var request = requestResult.Value;
            var takeOnReviewResult = request.TakeOnReview(command.AdminId, discussionId);

            if (takeOnReviewResult.IsFailure)
                return takeOnReviewResult.Error.ToErrorList();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();

            return discussionId;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            transaction.Rollback();
            return Error.Failure("internal.server.error", "Something went wrong").ToErrorList();
        }
    }
}