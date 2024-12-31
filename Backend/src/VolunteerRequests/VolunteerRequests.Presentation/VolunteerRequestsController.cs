using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.Core.Dtos.VolunteerRequests;
using PetProject.Framework;
using PetProject.Framework.Authorization;
using VolunteerRequests.Application.Mappers;
using VolunteerRequests.Contracts.Requests;

namespace VolunteerRequests.Presentation;

public class VolunteerRequestsController : ApplicationController
{
    private readonly IMediator _mediator;

    public VolunteerRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Permission(Permissions.VolunteerRequest.Read)]
    [HttpGet("new")]
    public async Task<ActionResult<List<VolunteerRequestDto>>> GetNewRequests(
        [FromQuery] GetNewVolunteerRequestsRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    [Permission(Permissions.VolunteerRequest.Read)]
    [HttpGet("admin")]
    public async Task<ActionResult<List<VolunteerRequestDto>>> GetRequestsByAdminId(
        [FromQuery] GetVolunteerRequestsByAdminIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Permission(Permissions.VolunteerRequest.Read)]
    [HttpGet("user")]
    public async Task<ActionResult<List<VolunteerRequestDto>>> GetRequestsByUserId(
        [FromQuery] GetVolunteerRequestsByUserIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    [Permission(Permissions.VolunteerRequest.Create)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [Permission(Permissions.VolunteerRequest.Update)]
    [HttpPut("{requestId}/sendForRevision")]
    public async Task<ActionResult> SendForRevision(
        [FromRoute] Guid requestId,
        [FromBody] SendForRevisionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(requestId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
     
    [Permission(Permissions.VolunteerRequest.Update)]
    [HttpPut("{requestId}/approve")]
    public async Task<ActionResult> Approve(
        [FromRoute] Guid requestId,
        [FromBody] ApproveVolunteerRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(requestId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [Permission(Permissions.VolunteerRequest.Update)]
    [HttpPut("{requestId}/reject")]
    public async Task<ActionResult> Reject(
        [FromRoute] Guid requestId,
        [FromBody] RejectVolunteerRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(requestId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [Permission(Permissions.VolunteerRequest.Update)]
    [HttpPut("{requestId}/TakeOnReview")]
    public async Task<ActionResult> TakeOnReview(
        [FromRoute] Guid requestId,
        [FromBody] TakeVolunteerRequestOnReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(requestId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteerRequest.Update)]
    [HttpPut("{requestId}/Update")]
    public async Task<ActionResult> Update(
        [FromRoute] Guid requestId,
        [FromBody] UpdateVolunteerRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(requestId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
}