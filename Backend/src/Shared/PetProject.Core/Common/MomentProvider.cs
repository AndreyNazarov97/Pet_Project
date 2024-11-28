using PetProject.SharedKernel.Interfaces;

namespace PetProject.Core.Common;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}