namespace PetProject.VolunteerManagement.Domain.ValueObjects;

public record PetPhoto
{
    public Guid FileId { get; }
    public bool IsMain { get; init; }

    private PetPhoto()
    {
        
    }
    public PetPhoto(Guid fileId)
    {
        FileId = fileId;
    }
}