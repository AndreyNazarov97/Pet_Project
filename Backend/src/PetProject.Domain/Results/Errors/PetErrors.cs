namespace PetProject.Domain.Results.Errors;

public class PetErrors : Error
{
    public PetErrors(string code, string description) : base(code, description)
    {
    }
    
    public static PetErrors NameRequired => new(nameof(NameRequired), "Name is required");
    
    public static PetErrors NameTooLong => new(nameof(NameTooLong), "Name is too long");

    public static PetErrors TypeRequired => new(nameof(TypeRequired), "Type is required");
    
    public static PetErrors TypeTooLong => new(nameof(TypeTooLong), "Type is too long");
    
    public static PetErrors DescriptionRequired => new(nameof(DescriptionRequired), "Description is required");
    
    public static PetErrors DescriptionTooLong => new(nameof(DescriptionTooLong), "Description is too long");
    
    public static PetErrors BreedNameRequired => new(nameof(BreedNameRequired), "BreedName is required");
    
    public static PetErrors BreedNameTooLong => new(nameof(BreedNameTooLong), "BreedName is too long");
    
    public static PetErrors ColorRequired => new(nameof(ColorRequired), "Color is required");
    
    public static PetErrors ColorTooLong => new(nameof(ColorTooLong), "Color is too long");
    
    public static PetErrors HealthInfoRequired => new(nameof(HealthInfoRequired), "HealthInfo is required");
    
    public static PetErrors HealthInfoTooLong => new(nameof(HealthInfoTooLong), "HealthInfo is too long");
    
    public static PetErrors SpeciesIdRequired => new(nameof(SpeciesIdRequired), "Species id is required");

    public static PetErrors WeightInvalid => new(nameof(WeightInvalid), "Weight is invalid");
    
    public static PetErrors HeightInvalid => new(nameof(HeightInvalid), "Height is invalid");
    
}