using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class PetPhoto : Entity<PetPhotoId>
{
    private PetPhoto(){}
    private PetPhoto(PetPhotoId id, string path, bool isMain)
    : base(id)
    {
        Path = path;
        IsMain = isMain;
    }
    
    public string Path { get; private set; }
    public bool IsMain { get; private set; }

    public void SetAsMain() => IsMain = true;
    public void SetAsNotMain() => IsMain = false;
    
    public static Result<PetPhoto> Create(PetPhotoId id, string path, bool isMain)
    {
        if(string.IsNullOrWhiteSpace(path) || path.Length > Constants.MAX_PATH_LENGTH)
            return Result<PetPhoto>.Failure(new("Invalid path", $"{nameof(path)} cannot be null or empty or longer than {Constants.MAX_PATH_LENGTH} characters."));
        
        var petPhoto = new PetPhoto(id, path, isMain);
        return Result<PetPhoto>.Success(petPhoto);
    }
}