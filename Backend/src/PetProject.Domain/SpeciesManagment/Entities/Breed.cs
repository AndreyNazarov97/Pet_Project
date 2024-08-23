using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.SpeciesManagment.Entities;
/// <summary>
/// Порода 
/// </summary>
public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id) {}

    public Breed(BreedId id, NotNullableString name) : base(id)
    {
        Name = name;
    }
    
    public NotNullableString Name { get; private set; }
    
}