namespace PetProject.SharedKernel.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}