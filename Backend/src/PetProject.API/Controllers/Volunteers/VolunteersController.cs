using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Volunteers.Requests;
using PetProject.API.Extensions;
using PetProject.Application.VolunteersManagement.AddPetPhoto;
using PetProject.Application.VolunteersManagement.CreatePet;
using PetProject.Application.VolunteersManagement.CreateVolunteer;
using PetProject.Application.VolunteersManagement.UpdateRequisites;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;
using PetProject.Application.VolunteersManagement.UpdateVolunteer;
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
        [FromServices] IValidator<CreateVolunteerCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerRequest request,
        [FromServices] IValidator<UpdateVolunteerCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateRequisitesRequest request,
        [FromServices] IValidator<UpdateRequisitesCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateSocialLinksRequest request,
        [FromServices] IValidator<UpdateSocialLinksCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult<PetId>> CreatePet(
        [FromRoute] Guid volunteerId,
        [FromForm] CreatePetRequest request,
        [FromServices] IValidator<CreatePetCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
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
        [FromServices] AddPetPhotoHandler handler,
        [FromServices] IValidator<AddPetPhotoCommand> validator,
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

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();


        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}