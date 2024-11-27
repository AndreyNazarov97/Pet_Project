using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SpeciesManagement.Application.Repository;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.GetSpeciesList;

public class GetSpeciesListQueryHandler : IRequestHandler<GetSpeciesListQuery, Result<SpeciesDto[], ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;

    public GetSpeciesListQueryHandler(
        ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }

    public async Task<Result<SpeciesDto[], ErrorList>> Handle(GetSpeciesListQuery request,
        CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel()
        {
            Offset = request.Offset,
            Limit = request.Limit
        };
        var species = await _speciesRepository.Query(speciesQuery, cancellationToken);
        
        return species;
    }
}