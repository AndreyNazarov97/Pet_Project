using PetProject.SharedKernel.Shared.Common;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.Species.Domain.Entities;

namespace PetProject.Species.Domain.Aggregate;

public class Species : AggregateRoot<SpeciesId>
{
    private bool _isDeleted;
    private readonly List<Breed> _breeds = [];
    
    protected Species(SpeciesId id) : base(id) { }
    
    public SpeciesName Name { get; private set; }
    public IReadOnlyCollection<Breed> Breeds => _breeds;

    public Species(SpeciesId id, SpeciesName name, List<Breed>? breeds)
        : base(id)
    {
        Name = name;
        if (breeds != null) AddBreeds(breeds);
    }
    
    public void AddBreeds(List<Breed> breeds) => _breeds.AddRange(breeds);
    
    public void RemoveBreed(Breed breed) => _breeds.Remove(breed);

}