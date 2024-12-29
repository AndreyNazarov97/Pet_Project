using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Database;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;
using PetProject.Discussions.Application.Interfaces;
using PetProject.Discussions.Domain.ValueObjects;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;

public class DeleteMessageHandler : IRequestHandler<DeleteMessageCommand, UnitResult<ErrorList>>
{
    private readonly IDiscussionsRepository _discussionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMessageHandler(
        IDiscussionsRepository discussionsRepository,
        [FromKeyedServices(Constants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionsRepository = discussionsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var discussionId = DiscussionId.Create(request.DiscussionId);
        
        var discussionResult = await _discussionsRepository.GetById(discussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var messageId = MessageId.Create(request.MessageId);
        
        var deleteMessageResult = discussionResult.Value.DeleteMessage(request.UserId, messageId);
        if(deleteMessageResult.IsFailure)
            return deleteMessageResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}