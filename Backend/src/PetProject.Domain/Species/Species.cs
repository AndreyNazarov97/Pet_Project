using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement;

namespace PetProject.Domain.Species;

public class Species : AggregateRoot<SpeciesId>
{
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

}