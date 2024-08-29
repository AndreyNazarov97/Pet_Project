﻿using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Species;

public record BreedName
{
    public string Value { get; }

    private BreedName(string value)
    {
        Value = value;
    }

    public static Result<BreedName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("BreedName");

        return new BreedName(value);
    }
}