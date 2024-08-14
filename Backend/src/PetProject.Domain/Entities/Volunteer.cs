using PetProject.Domain.Consts;
using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities;

public class Volunteer : Entity
{
    public FullName FullName { get; private set; }
    
    public string Description { get; private set; }
    
    public int Experience { get; private set; }
    
    public int PetsAdopted { get; private set; }
    
    public int PetsFoundHomeQuantity { get; private set; }
    
    public int PetsInTreatment { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; }

    public List<SocialNetwork> SocialNetworks { get; private set; } = [];

    public List<Requisite> Requisites { get; private set; } = [];

    #region Constructors
    private Volunteer()
    {
    }
    
    private Volunteer(
        FullName fullName, 
        string description, 
        int experience, 
        int petsAdopted, 
        int petsFoundHomeQuantity, 
        int petsInTreatment, 
        PhoneNumber phoneNumber) 
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        PetsAdopted = petsAdopted;
        PetsFoundHomeQuantity = petsFoundHomeQuantity;
        PetsInTreatment = petsInTreatment;
        PhoneNumber = phoneNumber;
    }
    
    public static Result<Volunteer> Create(
        FullName fullName, 
        string description, 
        int experience, 
        int petsAdopted, 
        int petsFoundHomeQuantity, 
        int petsInTreatment, 
        PhoneNumber phoneNumber)
    {
        if(string.IsNullOrWhiteSpace(description))
            return Result<Volunteer>.Failure(VolunteerErrors.DescriptionRequired);
        if(description.Length > VolunteerConsts.MaxDescriptionLength)
            return Result<Volunteer>.Failure(VolunteerErrors.DescriptionTooLong);
        if(experience < 0)
            return Result<Volunteer>.Failure(VolunteerErrors.ExperienceInvalid);
        if(petsAdopted < 0)
            return Result<Volunteer>.Failure(VolunteerErrors.PetsAdoptedInvalid);
        if(petsFoundHomeQuantity < 0)
            return Result<Volunteer>.Failure(VolunteerErrors.PetsFoundHomeQuantityInvalid);
        if(petsInTreatment < 0)
            return Result<Volunteer>.Failure(VolunteerErrors.PetsInTreatmentInvalid);
        
        
        
        return new Volunteer(fullName, description, experience, petsAdopted, petsFoundHomeQuantity, petsInTreatment, phoneNumber);
    }
    #endregion

    public void AddRequisite(Requisite requisite)
    {
        Requisites.Add(requisite);
    }

    public void AddSocialNetwork(SocialNetwork socialNetwork)
    {
        SocialNetworks.Add(socialNetwork);
    }
}