using CSharpFunctionalExtensions;

namespace PetProject.Domain.Shared.ValueObjects;

public record Experience
{
    public int Years { get; }

    private Experience(int years)
    {
        Years = years;
    }

    public static Result<Experience, Error> Create(int years)
    {
        if (years < 0)
            return Errors.General.ValueIsInvalid("Years");

        return new Experience(years);
    }
}