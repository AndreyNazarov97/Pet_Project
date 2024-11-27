using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.SharedKernel.Shared;
using PetProject.SpeciesManagement.Application.Extensions;
using PetProject.SpeciesManagement.Application.Repository;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.DeleteBreed;

public class DeleteBreedHandler : IRequestHandler<DeleteBreedCommand, UnitResult<ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBreedHandler(
        ISpeciesRepository speciesRepository,
        IReadRepository readRepository,
        IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _readRepository = readRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeleteBreedCommand request, CancellationToken cancellationToken)
    {
        var speciesQuery = new SpeciesQueryModel
        {
            SpeciesName = request.SpeciesName
        };
        var species = (await _speciesRepository.Query(speciesQuery, cancellationToken)).SingleOrDefault();
        if (species == null)
            return Errors.General.NotFound().ToErrorList();

        var existedBreed = species.Breeds.FirstOrDefault(x => x.Name == request.BreedName);
        if (existedBreed == null)
            return Errors.General.NotFound().ToErrorList();

        var volunteerQuery = new VolunteerQueryModel()
        {
            BreedNames = [request.BreedName]
        };
        var volunteers = await _readRepository.Query(volunteerQuery, cancellationToken);
        if (volunteers.Length > 0)
            return Error.Conflict("species.is.used.by.volunteers", "Species is used by volunteers").ToErrorList();

        var speciesEntity = species.ToEntity();
        var breedEntity = existedBreed.ToEntity();
        speciesEntity.RemoveBreed(breedEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}