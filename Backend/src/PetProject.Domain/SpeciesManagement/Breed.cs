﻿using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.SpeciesManagement.ValueObjects;

namespace PetProject.Domain.SpeciesManagement;

public class Breed : Shared.Common.Entity<BreedId>
{
    private bool _isDeleted;
    public BreedName BreedName { get; } = null!;

    protected Breed(BreedId id) : base(id) { }

    public Breed(BreedId id, BreedName breedName) : base(id)
    {
        BreedName = breedName;
    }
}