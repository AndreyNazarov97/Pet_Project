using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.Discussions.Application.DiscussionsManagement.Queries.GetByRelationId;
using PetProject.Framework;

namespace PetProject.Discussions.Presentation;

public class DiscussionsController : ApplicationController
{
    private readonly IMediator _mediator;

    public DiscussionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("discussions")]
    public async Task<IActionResult> GetDiscussionsByRelationId(
        [FromQuery] Guid relationId,
        CancellationToken cancellationToken)
    {
        var query = new GetByRelationIdQuery(){ RelationId = relationId};
        
        var result = await _mediator.Send(query, cancellationToken);        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

}
    
