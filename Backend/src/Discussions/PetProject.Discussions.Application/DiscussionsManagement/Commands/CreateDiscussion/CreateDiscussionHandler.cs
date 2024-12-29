using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Accounts.Contracts;
using PetProject.Discussions.Application.Interfaces;
using PetProject.Discussions.Domain.Aggregate;
using PetProject.Discussions.Domain.Entity;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.CreateDiscussion;

public class CreateDiscussionHandler : IRequestHandler<CreateDiscussionCommand, Result<Discussion, ErrorList>>
{
    private readonly IAccountsContract _accountsContract;
    private readonly IDiscussionsRepository _discussionsRepository;

    public CreateDiscussionHandler(
        IAccountsContract accountsContract,
        IDiscussionsRepository discussionsRepository)
    {
        _accountsContract = accountsContract;
        _discussionsRepository = discussionsRepository;
    }

    public async Task<Result<Discussion, ErrorList>> Handle(CreateDiscussionCommand request,
        CancellationToken cancellationToken)
    {
        
        var firstMember = await _accountsContract.GetUserById(request.FirstMemberId, cancellationToken);
        if (firstMember.IsFailure)
            return firstMember.Error;
        
        var secondMember = await _accountsContract.GetUserById(request.SecondMemberId, cancellationToken);
        if (secondMember.IsFailure)
            return secondMember.Error;
        var members = Members.Create(request.FirstMemberId, request.SecondMemberId).Value;

        var isDiscussionExists = await _discussionsRepository.GetByRelationId(request.RelationId, cancellationToken);
        if (isDiscussionExists.IsSuccess)
            return Errors.General.AlreadyExist("Discussion").ToErrorList();
        
        var discussion = Discussion.Create(request.RelationId, members);

        if (discussion.IsFailure)
            return discussion.Error.ToErrorList();

        var result = await _discussionsRepository.Add(discussion.Value, cancellationToken);
        return result;
    }
}