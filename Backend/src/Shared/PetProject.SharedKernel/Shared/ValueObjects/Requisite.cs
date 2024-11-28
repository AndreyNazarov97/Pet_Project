using CSharpFunctionalExtensions;

namespace PetProject.SharedKernel.Shared.ValueObjects;

public record Requisite
{
    public string Title { get; }
    public string Description { get; }
    
    private Requisite() { }

    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public static Result<Requisite, Error> Create(string requisiteName, string requisiteDescription)
    {
        if (string.IsNullOrWhiteSpace(requisiteName) || requisiteName.Length > Constants.Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Requisite");

        if (string.IsNullOrWhiteSpace(requisiteDescription) ||
            requisiteDescription.Length > Constants.Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Requisite");

        return new Requisite(requisiteName, requisiteDescription);
    }
}