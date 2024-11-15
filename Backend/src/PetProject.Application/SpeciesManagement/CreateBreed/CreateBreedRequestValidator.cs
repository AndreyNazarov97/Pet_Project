﻿using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Application.SpeciesManagement.CreateBreed;

public class CreateBreedRequestValidator : AbstractValidator<CreateBreedRequest>
{
    public CreateBreedRequestValidator()
    {
        RuleFor(c => c.BreedName)
            .MustBeValueObject(BreedName.Create);

        RuleFor(c => c.SpeciesName)
            .MustBeValueObject(SpeciesName.Create);
    }
}