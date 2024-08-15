using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class VolunteerDetails : Entity<Guid>
{
    private readonly List<Requisite> _requisites = [];
    private readonly List<SocialNetwork> _socialNetworks = [];

    private VolunteerDetails(){}
    private VolunteerDetails(
        int experience,
        int petsAdopted,
        int petsFoundHomeQuantity,
        int petsInTreatment,
        List<Requisite> requisites, 
        List<SocialNetwork> socialNetworks)
    {
        Experience = experience;
        PetsAdopted = petsAdopted;
        PetsFoundHomeQuantity = petsFoundHomeQuantity;
        PetsInTreatment = petsInTreatment;

        _requisites = requisites;
        _socialNetworks = socialNetworks;
    }

    public int Experience { get;  }
    public int PetsAdopted { get;  }
    public int PetsFoundHomeQuantity { get;  }
    public int PetsInTreatment { get; }
    public IReadOnlyCollection<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyCollection<Requisite> Requisites => _requisites;

    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => _socialNetworks.AddRange(socialNetworks);
    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);

    public static Result<VolunteerDetails> Create(
        int experience,
        int petsAdopted,
        int petsFoundHomeQuantity,
        int petsInTreatment,
        List<Requisite>? requisites, 
        List<SocialNetwork>? socialNetworks)
    {
        if (experience < Constants.MIN_VALUE)
            return Result<VolunteerDetails>.Failure(new("Invalid experience",
                $"{nameof(experience)} cannot be less than {Constants.MIN_VALUE}"));

        if (petsAdopted < Constants.MIN_VALUE)
            return Result<VolunteerDetails>.Failure(new("Invalid petsAdopted",
                $"{nameof(petsAdopted)} cannot be less than {Constants.MIN_VALUE}"));

        if (petsFoundHomeQuantity < Constants.MIN_VALUE)
            return Result<VolunteerDetails>.Failure(new("Invalid petsFoundHomeQuantity",
                $"{nameof(petsFoundHomeQuantity)} cannot be less than {Constants.MIN_VALUE}"));

        if (petsInTreatment < Constants.MIN_VALUE)
            return Result<VolunteerDetails>.Failure(new("Invalid petsInTreatment",
                $"{nameof(petsInTreatment)} cannot be less than {Constants.MIN_VALUE}"));
        
        var details = new VolunteerDetails(
            experience,
            petsAdopted,
            petsFoundHomeQuantity,
            petsInTreatment,
            requisites ?? [],
            socialNetworks ?? []);
        return Result<VolunteerDetails>.Success(details);
    }
    
}