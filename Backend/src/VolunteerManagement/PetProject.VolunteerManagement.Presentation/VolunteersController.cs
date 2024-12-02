﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.Core.Dtos;
using PetProject.Framework;
using PetProject.Framework.Processors;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.VolunteersManagement.AddPetPhoto;
using PetProject.VolunteerManagement.Application.VolunteersManagement.GetPetById;
using PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteer;
using PetProject.VolunteerManagement.Application.VolunteersManagement.SoftDeletePet;
using PetProject.VolunteerManagement.Application.VolunteersManagement.SoftDeleteVolunteer;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Presentation.Requests;

namespace PetProject.VolunteerManagement.Presentation;

public class VolunteersController : ApplicationController
{
    private readonly IMediator _mediator;

    public VolunteersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{volunteerId:guid}")]
    public async Task<ActionResult<Volunteer>> Get(
        [FromRoute] Guid volunteerId,
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteerQuery { VolunteerId = volunteerId };

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<VolunteerDto>>> GetList(
        [FromQuery] GetVolunteersListRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
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

    [HttpDelete("{volunteerId:guid}")]
    public async Task<ActionResult> DeleteVolunteer(
        [FromRoute] Guid volunteerId,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeleteVolunteerCommand
        {
            VolunteerId = volunteerId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet("pets")]
    public async Task<ActionResult<PetDto[]>> GetPets(
        [FromQuery] GetPetsRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("pets/{petId:guid}")]
    public async Task<ActionResult<PetDto>> GetPet(
        [FromRoute] Guid petId,
        CancellationToken cancellationToken)
    {
        var query = new GetPetByIdQuery
        {
            PetId = petId
        };

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();    

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pets")]
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

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}")]
    public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photo")]
    public async Task<ActionResult<List<FilePath>>> AddPetPhoto(
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

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/photo")]
    public async Task<ActionResult> SetMainPetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] SetMainPetPhotoRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok("Success");
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/status")]
    public async Task<ActionResult> ChangePetStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] ChangePetStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok("Success");
    }

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}")]
    public async Task<ActionResult> DeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken)
    {
        var command = new SoftDeletePetCommand()
        {
            VolunteerId = volunteerId,

            PetId = petId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/photo/")]
    public async Task<ActionResult> DeletePetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetPhotoRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId, petId);
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();    

        return Ok();
    }
}