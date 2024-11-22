using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Species.Requests;
using PetProject.API.Extensions;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.SpeciesManagement.DeleteBreed;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    private readonly IMediator _mediator;

    public SpeciesController(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<ActionResult<SpeciesId>> CreateSpecies(
        [FromBody] CreateSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{speciesName}/breeds")]
    public async Task<ActionResult<BreedId>> CreateBreed(
        [FromRoute] string speciesName,
        [FromBody] CreateBreedRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(speciesName);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete]
    public async Task<ActionResult> DeleteSpecies(
        [FromBody] DeleteSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpDelete("{speciesName}/breeds/{breedName}")]
    public async Task<ActionResult> DeleteBreed(
        [FromRoute] string speciesName,
        [FromRoute] string breedName,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBreedCommand
        {
            SpeciesName = speciesName,
            BreedName = breedName
        };
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<SpeciesDto>>> GetSpeciesList(
        [FromQuery] GetSpeciesListRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}