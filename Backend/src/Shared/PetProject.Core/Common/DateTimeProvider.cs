using PetProject.SharedKernel.Interfaces;

namespace PetProject.Core.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}