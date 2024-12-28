namespace PetProject.SharedKernel.Shared.EntityIds;

public record VolunteerRequestId
{
    public Guid Id { get; }

    private VolunteerRequestId(Guid id) => Id = id;

    public static VolunteerRequestId NewId() => new (Guid.NewGuid());
    public static VolunteerRequestId Empty() => new (Guid.Empty);
    public static VolunteerRequestId Create(Guid id) => new (id);
    public static implicit operator Guid(VolunteerRequestId volunteerRequestId) => volunteerRequestId.Id;
}