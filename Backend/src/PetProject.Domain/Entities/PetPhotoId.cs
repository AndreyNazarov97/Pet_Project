namespace PetProject.Domain.Entities;

public class PetPhotoId
{
    private PetPhotoId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static PetPhotoId NewPetPhotoId() => new(Guid.NewGuid());
    public static PetPhotoId Empty() => new(Guid.Empty);
}