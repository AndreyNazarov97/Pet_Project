using PetProject.Domain.Consts;
using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities.ValueObjects;

public record Requisite
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    
    private Requisite() { }
    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }
    
    public static Result<Requisite> Create(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<Requisite>.Failure(RequisiteErrors.TitleRequired);
        if (title.Length > RequisiteConsts.TitleMaxLength)
            return Result<Requisite>.Failure(RequisiteErrors.TitleTooLong);
        if (string.IsNullOrWhiteSpace(description))
            return Result<Requisite>.Failure(RequisiteErrors.DescriptionRequired);
        if (description.Length > RequisiteConsts.DescriptionMaxLength)
            return Result<Requisite>.Failure(RequisiteErrors.DescriptionTooLong);
        
        return new Requisite(title, description);
    }
}
