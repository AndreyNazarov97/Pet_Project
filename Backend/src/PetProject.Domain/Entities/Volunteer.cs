using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class Volunteer : Entity<VolunteerId>
{
    private Volunteer()
    {
    }

    private Volunteer(
        VolunteerId id,
        FullName fullName,
        string description,
        PhoneNumber phoneNumber,
        VolunteerDetails details)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        Details = details;
    }

    public FullName FullName { get; private set; }
    public string Description { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public VolunteerDetails Details { get; private set; }


    public void UpdateDetails(VolunteerDetails details) => Details = details;
    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => Details.AddSocialNetworks(socialNetworks);
    public void AddRequisites(List<Requisite> requisites) => Details.AddRequisites(requisites); 
    //TODO добавить логику подсчета животных

    public static Result<Volunteer> Create(
        VolunteerId volunteerId,
        FullName fullName,
        string description,
        PhoneNumber phoneNumber,
        VolunteerDetails details
    )
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Result<Volunteer>.Failure(new("Invalid description",
                $"{nameof(description)} cannot be null or empty or longer than {Constants.MAX_LONG_TEXT_LENGTH} characters."));
        
        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            phoneNumber,
            details
        );
        
        return Result<Volunteer>.Success(volunteer);
    }
}