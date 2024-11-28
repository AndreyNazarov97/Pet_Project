using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.GetBreedsList;

public class GetBreedsListHandler : IRequestHandler<GetBreedsListQuery, Result<BreedDto[],ErrorList>>
{
    private readonly IReadRepository _readRepository;

    public GetBreedsListHandler(
        IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    
    public async Task<Result<BreedDto[], ErrorList>> Handle(GetBreedsListQuery request, CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = request.SpeciesName,
            Offset = request.Offset,
            Limit = request.Limit
        };
        var species = (await _readRepository.QuerySpecies(speciesQuery, cancellationToken)).SingleOrDefault();

        if (species == null)
            return Errors.General.NotFound().ToErrorList();
        
        return species.Breeds.ToArray();
    }
}