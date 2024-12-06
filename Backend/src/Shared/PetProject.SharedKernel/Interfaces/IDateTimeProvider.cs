namespace PetProject.SharedKernel.Interfaces;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}