using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class VolunteerId : BaseId<VolunteerId>
{
    protected VolunteerId(Guid id) : base(id)
    {
    }
}