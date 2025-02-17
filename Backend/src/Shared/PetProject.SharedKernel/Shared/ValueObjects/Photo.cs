namespace PetProject.SharedKernel.Shared.ValueObjects;

public record Photo
{
    public Guid FileId { get; }
    public bool IsMain { get; init; }

    private Photo()
    {
        
    }
    public Photo(Guid fileId )
    {
        FileId = fileId;
    }
}