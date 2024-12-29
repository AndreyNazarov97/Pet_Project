using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Database;
using PetProject.Discussions.Application.Interfaces;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.CloseDiscussion;

public class CloseDiscussionHandler : IRequestHandler<CloseDiscussionCommand, UnitResult<ErrorList>>
{
    private readonly IDiscussionsRepository _discussionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseDiscussionHandler(
        IDiscussionsRepository discussionsRepository,
        [FromKeyedServices(Constants.Context.Discussions)]
        IUnitOfWork unitOfWork)
    {
        _discussionsRepository = discussionsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(CloseDiscussionCommand request, CancellationToken cancellationToken)
    {
        var discussionId = DiscussionId.Create(request.DiscussionId);

        var discussionResult = await _discussionsRepository.GetById(discussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var closeDiscussionResult = discussionResult.Value.CloseDiscussion(request.UserId);
        if (closeDiscussionResult.IsFailure)
            return closeDiscussionResult.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}