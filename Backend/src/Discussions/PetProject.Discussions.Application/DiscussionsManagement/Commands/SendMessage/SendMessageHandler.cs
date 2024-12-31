using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Database;
using PetProject.Discussions.Application.Interfaces;
using PetProject.Discussions.Domain.Entity;
using PetProject.Discussions.Domain.ValueObjects;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, Result<Guid, ErrorList>>
{
    private readonly IDiscussionsRepository _discussionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendMessageHandler(
        IDiscussionsRepository discussionsRepository,
        [FromKeyedServices(Constants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionsRepository = discussionsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var discussionId = DiscussionId.Create(request.DiscussionId);
        
        var discussionResult = await _discussionsRepository.GetById(discussionId, cancellationToken);
        if (discussionResult.IsFailure)
            return discussionResult.Error.ToErrorList();

        var messageResult = Message.Create(Text.Create(request.Text).Value, request.UserId);
        if (messageResult.IsFailure)
            return messageResult.Error.ToErrorList();
        
        var sendMessageResult = discussionResult.Value.AddMessage(messageResult.Value);
        if(sendMessageResult.IsFailure)
            return sendMessageResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return messageResult.Value.Id.Id;
    }
}