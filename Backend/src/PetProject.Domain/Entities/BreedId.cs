using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class BreedId : BaseId<BreedId>
{
    protected BreedId(Guid id) : base(id)
    {
    }
}