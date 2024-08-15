using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class SpeciesId : BaseId<SpeciesId>
{
    protected SpeciesId(Guid id) : base(id)
    {
    }
}