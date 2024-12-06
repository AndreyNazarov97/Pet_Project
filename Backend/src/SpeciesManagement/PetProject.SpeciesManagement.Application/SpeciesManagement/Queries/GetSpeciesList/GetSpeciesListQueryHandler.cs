using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Queries.GetSpeciesList;

public class GetSpeciesListQueryHandler : IRequestHandler<GetSpeciesListQuery, Result<SpeciesDto[], ErrorList>>
{
    private readonly IReadRepository _readRepository;

    public GetSpeciesListQueryHandler(
        IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<Result<SpeciesDto[], ErrorList>> Handle(GetSpeciesListQuery request,
        CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel()
        {
            Offset = request.Offset,
            Limit = request.Limit
        };
        var species = await _readRepository.QuerySpecies(speciesQuery, cancellationToken);
        
        return species;
    }
}