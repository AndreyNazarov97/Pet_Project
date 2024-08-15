using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class PetPhotoId : BaseId<PetPhotoId>
{
    protected PetPhotoId(Guid id) : base(id)
    {
    }
}