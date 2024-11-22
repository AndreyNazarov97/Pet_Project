using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Species.Requests;
using PetProject.API.Extensions;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.SpeciesManagement;
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

    [HttpGet]
    public async Task<ActionResult<List<SpeciesDto>>> GetSpecies(
        [FromServices] ISpeciesRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetAll(cancellationToken);

        return Ok(result.Value.Select(x =>
                new SpeciesDto(
                    x.Name.Value.ToString(),
                    x.Breeds.Select(y => y.BreedName.Value.ToString()).ToList()))
            .ToList());
    }
}