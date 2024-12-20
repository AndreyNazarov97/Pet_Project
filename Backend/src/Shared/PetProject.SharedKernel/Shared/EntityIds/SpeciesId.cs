﻿namespace PetProject.SharedKernel.Shared.EntityIds;

public record SpeciesId
{
    public Guid Id { get; }

    private SpeciesId(Guid id) => Id = id;

    public static SpeciesId NewId() => new (Guid.NewGuid());
    public static SpeciesId Empty() => new (Guid.Empty);
    public static SpeciesId Create(Guid id) => new (id);
    public static implicit operator Guid(SpeciesId breedId) => breedId.Id;
}