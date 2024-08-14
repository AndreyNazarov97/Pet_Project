namespace PetProject.Domain.Results.Errors;

public class RequisiteErrors : Error
{
    public RequisiteErrors(string code, string description) : base(code, description)
    {
    }
    
    public static RequisiteErrors TitleRequired => new(nameof(TitleRequired), "Title is required");
    public static RequisiteErrors TitleTooLong => new(nameof(TitleTooLong), "Title is too long");
    public static RequisiteErrors DescriptionRequired => new(nameof(DescriptionRequired), "Description is required");
    public static RequisiteErrors DescriptionTooLong => new(nameof(DescriptionTooLong), "Description is too long");
   
}