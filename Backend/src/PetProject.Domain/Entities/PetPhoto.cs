using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class PetPhoto : Entity<PetPhotoId>
{
    public string Path { get; private set; }
    
    public bool IsMain { get; private set; }

    private PetPhoto()
    {
    }
}