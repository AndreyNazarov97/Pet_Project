namespace PetProject.Domain.Shared.EntityIds;

public class PetPhotoId
{
    private PetPhotoId(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static PetPhotoId NewPetPhotoId() => new(Guid.NewGuid());
    public static PetPhotoId FromGuid(Guid id) => new(id);
    public static PetPhotoId Empty() => new(Guid.Empty);
    
    public override string ToString() => Id.ToString();
}