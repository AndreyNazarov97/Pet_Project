using FileService.Communication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.Core.Dtos;
using PetProject.Framework;
using PetProject.Framework.Authorization;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.AddPetPhoto;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeletePet;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.SoftDeleteVolunteer;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetPetById;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetVolunteer;
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

    [Permission(Permissions.Volunteer.Read)]
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

    //[Permission(Permissions.Volunteer.Read)]
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

    [Permission(Permissions.Volunteer.Create)]
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

    [Permission(Permissions.Volunteer.Update)]
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

    [Permission(Permissions.Volunteer.Delete)]
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

    [Permission(Permissions.Volunteer.Read)]
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

    [Permission(Permissions.Volunteer.Read)]
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

    [Permission(Permissions.Volunteer.Create)]
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

    [Permission(Permissions.Volunteer.Update)]
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

    [Permission(Permissions.Volunteer.Update)]
    [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photo")]
    public async Task<ActionResult<List<PhotoDto>>> AddPetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection photos,
        [FromServices] FileHttpClient fileHttpClient,
        CancellationToken cancellationToken)
    {
        var uploadFilesResponse = await fileHttpClient
            .UploadFilesAsync("pet-project", photos, cancellationToken);

        if (uploadFilesResponse.IsFailure)
            return Error.Failure("file.service.error", uploadFilesResponse.Error).ToResponse();
        
        var command = new AddPetPhotoCommand
        {
            VolunteerId = volunteerId,
            PetId = petId,
            FilesId = uploadFilesResponse.Value
                .Where(x => Constants.PhotoExtensions.Contains(Path.GetExtension(x.Key)))
                .Select(r => r.FileId).ToList()
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.Volunteer.Update)]
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

    [Permission(Permissions.Volunteer.Update)]
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

    [Permission(Permissions.Volunteer.Delete)]
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

    [Permission(Permissions.Volunteer.Delete)]
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