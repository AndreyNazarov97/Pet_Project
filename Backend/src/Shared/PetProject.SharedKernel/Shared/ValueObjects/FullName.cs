﻿using CSharpFunctionalExtensions;

namespace PetProject.SharedKernel.Shared.ValueObjects;

public record FullName
{
    public string Name { get; }
    public string Surname { get; }
    public string? Patronymic { get; }

    private FullName(string name, string surname, string? patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }

    public static Result<FullName, Error> Create(string name, string surname, string? patronymic = null)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Name");

        if (string.IsNullOrWhiteSpace(surname) || name.Length > Constants.Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Surname");

        if (patronymic?.Length > Constants.Constants.MIN_TEXT_LENGTH)
                return Errors.General.ValueIsInvalid("Surname");
        
        return new FullName(name, surname, patronymic);
    }
}