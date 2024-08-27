using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities.Details;

public record VolunteerDetails 
{
    protected VolunteerDetails(){}
    public VolunteerDetails(
        List<Requisite> requisites, 
        List<SocialNetwork> socialNetworks)
    {
        Requisites = requisites;
        SocialNetworks = socialNetworks;
    }
    
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
    public IReadOnlyList<Requisite> Requisites { get; }
}