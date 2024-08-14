namespace PetProject.Domain.Results.Errors;

public class VolunteerErrors : Error
{
    public VolunteerErrors(string code, string description) : base(code, description)
    {
    }
    
    public static VolunteerErrors DescriptionRequired => new(nameof(DescriptionRequired), "Description is required");

    public static VolunteerErrors DescriptionTooLong => new(nameof(DescriptionTooLong), "Description is too long");
    
    public static VolunteerErrors ExperienceInvalid => new(nameof(ExperienceInvalid), "Experience is invalid");

    public static VolunteerErrors PetsAdoptedInvalid => new(nameof(PetsAdoptedInvalid), "PetsAdopted is invalid");

    public static VolunteerErrors PetsFoundHomeQuantityInvalid => new(nameof(PetsFoundHomeQuantityInvalid), "PetsFoundHomeQuantity is invalid");

    public static VolunteerErrors PetsInTreatmentInvalid => new(nameof(PetsInTreatmentInvalid), "PetsInTreatment is invalid");
    
}