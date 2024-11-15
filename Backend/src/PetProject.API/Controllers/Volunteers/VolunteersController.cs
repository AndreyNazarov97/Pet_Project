using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Volunteers.Requests;
using PetProject.API.Extensions;
using PetProject.Application.Dto;
using PetProject.Application.Volunteers.AddPetPhoto;
using PetProject.Application.Volunteers.CreatePet;
using PetProject.Application.Volunteers.CreateVolunteer;
using PetProject.Application.Volunteers.UpdateRequisites;
using PetProject.Application.Volunteers.UpdateSocialLinks;
using PetProject.Application.Volunteers.UpdateVolunteer;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Infrastructure.Postgres.Processors;

namespace PetProject.API.Controllers.Volunteers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<VolunteerId>> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        [FromServices] IValidator<CreateVolunteerCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        // var validationResult = await validator.ValidateAsync(command, cancellationToken);
        // if (!validationResult.IsValid)
        //     return validationResult.ToValidationErrorResponse();
        
        var result = await handler.Execute(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerRequest request,
        [FromServices] UpdateVolunteerHandler handler,
        [FromServices] IValidator<UpdateVolunteerCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Execute(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateRequisitesRequest request,
        [FromServices] UpdateRequisitesHandler handler,
        [FromServices] IValidator<UpdateRequisitesCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Execute(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateSocialLinksRequest request,
        [FromServices] UpdateSocialLinksHandler handler,
        [FromServices] IValidator<UpdateSocialLinksCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Execute(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult<PetId>> CreatePet(
        [FromRoute] Guid volunteerId,
        [FromForm] CreatePetRequest request,
        [FromServices] CreatePetHandler handler,
        [FromServices] IValidator<CreatePetCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await handler.Handle(request.ToCommand(volunteerId), cancellationToken);

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