using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.Core.Dtos;
using PetProject.Framework;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SpeciesManagement.Application.SpeciesManagement.Commands.DeleteBreed;
using PetProject.SpeciesManagement.Presentation.Requests;

namespace PetProject.SpeciesManagement.Presentation;

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
    
    [HttpGet("{speciesName}/breeds")]
    public async Task<ActionResult<List<BreedDto>>> GetBreedsList(
        [FromRoute] string speciesName,
        [FromQuery] GetBreedsListRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery(speciesName);

        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}