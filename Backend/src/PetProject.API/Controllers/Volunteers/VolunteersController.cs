using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Volunteers.Requests;
using PetProject.API.Extensions;
using PetProject.Application.VolunteersManagement.AddPetPhoto;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Infrastructure.Postgres.Processors;

namespace PetProject.API.Controllers.Volunteers;

public class VolunteersController : ApplicationController
{
    private readonly IMediator _mediator;

    public VolunteersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult<VolunteerId>> Create(
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateSocialLinksRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult<PetId>> CreatePet(
        [FromRoute] Guid volunteerId,
        [FromForm] CreatePetRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photo")]
    public async Task<ActionResult> AddPetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection photos,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();
        var filesDto = fileProcessor.Process(photos);
        
        var command = new AddPetPhotoCommand
        {
            VolunteerId = volunteerId,
            PetId = petId,
            Photos = filesDto
        };
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}