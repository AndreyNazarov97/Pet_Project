namespace PetProject.SharedKernel.Shared.ValueObjects;

public record Photo
{
    public FilePath Path { get; }
    public bool IsMain { get; init; }

    private Photo()
    {
        
    }
    public Photo(FilePath path)
    {
        Path = path;
    }
}