﻿namespace PetProject.SharedKernel.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}