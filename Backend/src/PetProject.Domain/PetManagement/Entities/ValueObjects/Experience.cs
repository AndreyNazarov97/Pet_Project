using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities.ValueObjects;

public class Experience : ValueObject
{
    private Experience(){}

    public Experience(int value)
    {
        Value = value;
    }
    public int Value { get; }

    public static Result<Experience> Create(int experience)
    {
        if (experience < Constants.MIN_VALUE)
            return Errors.General.ValueIsInvalid(nameof(experience));
        
        return Result<Experience>.Success(new Experience(experience));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}