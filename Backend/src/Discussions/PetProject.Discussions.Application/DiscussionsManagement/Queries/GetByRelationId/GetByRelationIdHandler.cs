using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject.Core.Dtos.Discussions;
using PetProject.Discussions.Application.Interfaces;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Queries.GetByRelationId;

public class GetByRelationIdHandler : IRequestHandler<GetByRelationIdQuery, Result<DiscussionDto, ErrorList>>
{
    private readonly IReadDbContext _readDbContext;

    public GetByRelationIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<DiscussionDto, ErrorList>> Handle(GetByRelationIdQuery request, CancellationToken cancellationToken)
    {
        var discussion = await _readDbContext.Discussions
            .Include(x => x.Members)
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(d => d.RelationId == request.RelationId, cancellationToken);

        if (discussion is null)
            return Errors.General.Null().ToErrorList();

        return discussion;
    }
}