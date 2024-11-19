namespace PetProject.Infrastructure.Postgres.Abstractions;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}

internal class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}