using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record Position
{
    public int Value { get; }

    private Position(int value)
    {
        Value = value;
    }

    public static Result<Position, Error> Create(int value)
    {
        if (value <= 0)
        {
            return Errors.General.ValueIsInvalid(nameof(Position));
        }
        
        return new Position(value);
    }
}