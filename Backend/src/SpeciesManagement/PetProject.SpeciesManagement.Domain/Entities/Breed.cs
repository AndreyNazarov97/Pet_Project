﻿using PetProject.SharedKernel.Shared.Common;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.SpeciesManagement.Domain.Entities;

public class Breed : SoftDeletableEntity<BreedId>
{
    public BreedName BreedName { get; } = null!;

    protected Breed(BreedId id) : base(id) { }

    public Breed(BreedId id, BreedName breedName) : base(id)
    {
        BreedName = breedName;
    }
}