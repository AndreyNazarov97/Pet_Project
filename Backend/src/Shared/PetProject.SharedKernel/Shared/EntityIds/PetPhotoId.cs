﻿namespace PetProject.SharedKernel.Shared.EntityIds;

public class PetPhotoId
{
    public Guid Id { get; }
    private PetPhotoId(Guid id) => Id = id;

    public static PetPhotoId NewId() => new (Guid.NewGuid());
    public static PetPhotoId Empty() => new (Guid.Empty);
    public static PetPhotoId Create(Guid id) => new (id);

    public static implicit operator Guid(PetPhotoId breedId) => breedId.Id;
}