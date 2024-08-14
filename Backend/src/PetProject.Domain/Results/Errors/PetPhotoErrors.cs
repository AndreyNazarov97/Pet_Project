namespace PetProject.Domain.Results.Errors;

public class PetPhotoErrors : Error
{
    public PetPhotoErrors(string code, string description) : base(code, description)
    {
    }
    
    public static PetPhotoErrors PathRequired => new(nameof(PathRequired), "Path is required");
}
   