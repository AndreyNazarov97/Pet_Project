using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SpeciesManagement.Application.Repository;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.GetBreedsList;

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