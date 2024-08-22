namespace PetProject.Domain.Shared.ValueObjects;

public abstract class ValueObject
{
    public abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(default(int), (hashcode, value) =>
            HashCode.Combine(hashcode, value.GetHashCode()));

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;
        return a.Equals(b);
    }
    public static bool operator !=(ValueObject? a, ValueObject? b) => !Equals(a, b);
}