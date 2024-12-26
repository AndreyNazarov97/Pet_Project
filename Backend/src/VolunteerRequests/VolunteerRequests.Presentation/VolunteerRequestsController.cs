using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.Framework;
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
}