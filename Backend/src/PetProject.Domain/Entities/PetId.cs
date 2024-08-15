using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class PetId : BaseId<PetId>
{
    protected PetId(Guid id) : base(id)
    {
    }
}