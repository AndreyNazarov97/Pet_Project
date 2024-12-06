namespace PetProject.SharedKernel.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}