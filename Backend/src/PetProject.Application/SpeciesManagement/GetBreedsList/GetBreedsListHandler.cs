using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Domain.Shared;

namespace PetProject.Application.SpeciesManagement.GetBreedsList;

public class GetBreedsListHandler : IRequestHandler<GetBreedsListQuery, Result<BreedDto[],ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;

    public GetBreedsListHandler(
        ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }
    
    public async Task<Result<BreedDto[], ErrorList>> Handle(GetBreedsListQuery request, CancellationToken cancellationToken)
    {
        var queryModel = new SpeciesQueryModel
        {
            SpeciesName = request.SpeciesName,
            Offset = request.Offset,
            Limit = request.Limit
        };
        var species = (await _speciesRepository
            .Query(queryModel, cancellationToken))
            .SingleOrDefault();

        if (species == null)
            return Errors.General.NotFound().ToErrorList();
        
        return species.Breeds.ToArray();
    }
}