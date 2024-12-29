using CSharpFunctionalExtensions;
using PetProject.Discussions.Application.DiscussionsManagement.Commands;
using PetProject.Discussions.Contracts;
using PetProject.Discussions.Contracts.Requests;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Presentation;

public class DiscussionContract : IDiscussionContract
{
    private readonly CreateDiscussionHandler _createDiscussionHandler;

    public DiscussionContract(CreateDiscussionHandler createDiscussionHandler)
    {
        _createDiscussionHandler = createDiscussionHandler;
    }
    
    public async Task<Result<Discussion, ErrorList>> Create(CreateDiscussionRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateDiscussionCommand
        {
            RelationId = request.RelationId,
            FirstMemberId = request.FirstMemberId,
            SecondMemberId = request.SecondMemberId
        };

        var result = await _createDiscussionHandler.Handle(command, cancellationToken);
        
        return result;
    }
}