using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.Application.Dto;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.SpeciesManagement.CreateBreed;
using PetProject.Application.SpeciesManagement.CreateSpecies;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.API.Controllers;

public class SpeciesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<SpeciesId>> CreateSpecies(
        [FromBody] CreateSpeciesCommand command,
        [FromServices] CreateSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{speciesName}/breeds")]
    public async Task<ActionResult<BreedId>> CreateBreed(
        [FromRoute] string speciesName,
        [FromBody] CreateBreedDto dto,
        [FromServices] CreateBreedHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new CreateBreedCommand
        {
            SpeciesName = speciesName,
            BreedName = dto.Name
        };
        var result = await handler.Handle(request, cancellationToken);

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