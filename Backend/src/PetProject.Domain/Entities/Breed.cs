using PetProject.Domain.Consts;
using PetProject.Domain.Results;

namespace PetProject.Domain.Entities;
/// <summary>
/// Порода 
/// </summary>
public class Breed : Entity
{
    public string Name { get; private set; }
    
    public Guid SpeciesId { get; private set; }

    private Breed()
    {
        
    }

    private Breed(string name, Guid speciesId)
    {
        Name = name;
        SpeciesId = speciesId;
    }

    public static Result<Breed> Create(string name, Guid speciesId)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Result<Breed>.Failure(Error.NameRequired);
        if(name.Length > BreedConsts.NameMaxLength)
            return Result<Breed>.Failure(Error.NameTooLong);
        if(name.Length < BreedConsts.NameMinLength)
            return Result<Breed>.Failure(Error.NameTooShort);

        if(speciesId == Guid.Empty || speciesId == default)
            return Result<Breed>.Failure(Error.IdRequired);
        
        return new Breed(name, speciesId);
    }
}