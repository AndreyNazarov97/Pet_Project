using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Database;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;
using PetProject.Discussions.Application.Interfaces;
using PetProject.Discussions.Domain.Entity;
using PetProject.Discussions.Domain.ValueObjects;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;

public class UpdateMessageHandler : IRequestHandler<UpdateMessageCommand, Result<Guid, ErrorList>>
{
    private readonly IDiscussionsRepository _discussionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMessageHandler(
        IDiscussionsRepository discussionsRepository,
        [FromKeyedServices(Constants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionsRepository = discussionsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var discussionId = DiscussionId.Create(request.DiscussionId);
        
        var discussionResult = await _discussionsRepository.GetById(discussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var messageId = MessageId.Create(request.MessageId);
        var text = Text.Create(request.Text).Value;
        
        var editMessageResult = discussionResult.Value.EditMessage(request.UserId, messageId, text);
        if(editMessageResult.IsFailure)
            return editMessageResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return messageId.Id;
    }
}