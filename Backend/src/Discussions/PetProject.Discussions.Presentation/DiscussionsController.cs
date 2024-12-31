using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.Discussions.Application.DiscussionsManagement.Queries.GetByRelationId;
using PetProject.Discussions.Application.Mappers;
using PetProject.Discussions.Contracts.Requests;
using PetProject.Framework;
using PetProject.Framework.Authorization;

namespace PetProject.Discussions.Presentation;

public class DiscussionsController : ApplicationController
{
    private readonly IMediator _mediator;

    public DiscussionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Permission(Permissions.Discussion.Read)]
    [HttpGet("discussions")]
    public async Task<IActionResult> GetDiscussionsByRelationId(
        [FromQuery] Guid relationId,
        CancellationToken cancellationToken)
    {
        var query = new GetByRelationIdQuery() { RelationId = relationId };

        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.Discussion.Update)]
    [HttpPut("close")]
    public async Task<IActionResult> CloseDiscussion(
        [FromBody] CloseDiscussionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [Permission(Permissions.Discussion.Update)]
    [HttpPut("send-message")]
    public async Task<IActionResult> SendMessage(
        [FromBody] SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Discussion.Update)]
    [HttpPut("update-message")]
    public async Task<IActionResult> UpdateMessage(
        [FromBody] UpdateMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();    

        return Ok();
    }

    [Permission(Permissions.Discussion.Update)]
    [HttpDelete("delete-message")]
    public async Task<IActionResult> DeleteMessage(
        [FromBody] DeleteMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    
}