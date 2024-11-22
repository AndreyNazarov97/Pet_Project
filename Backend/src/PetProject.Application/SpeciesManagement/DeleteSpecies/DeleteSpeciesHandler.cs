using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.DeleteSpecies;

public class DeleteSpeciesHandler : IRequestHandler<DeleteSpeciesCommand, UnitResult<ErrorList>>
{
    private readonly ISpeciesRepository _repository;

    public DeleteSpeciesHandler(
        ISpeciesRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(DeleteSpeciesCommand request, CancellationToken cancellationToken)
    {
        var speciesName = SpeciesName.Create(request.SpeciesName).Value;
        var speciesToDeleteResult = await _repository.GetByName(speciesName, cancellationToken);
        if(speciesToDeleteResult.IsFailure)
            return Errors.General.NotFound().ToErrorList();
        
        await _repository.Delete(speciesToDeleteResult.Value.Id, cancellationToken);
        return UnitResult.Success<ErrorList>();  
    }
}