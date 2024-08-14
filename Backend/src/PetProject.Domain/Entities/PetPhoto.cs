using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities;

public class PetPhoto : Entity
{
    public string Path { get; private set; }
    
    public bool IsMain { get; private set; }

    private PetPhoto()
    {
    }
    
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }
    
    public static Result<PetPhoto> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result<PetPhoto>.Failure(PetPhotoErrors.PathRequired);
        
        return new PetPhoto(path, isMain);
    }
}