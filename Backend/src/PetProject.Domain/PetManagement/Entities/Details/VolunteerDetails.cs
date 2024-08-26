using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.PetManagement.Entities.Details;

public class VolunteerDetails : ValueObject
{
    private readonly List<Requisite> _requisites = [];
    private readonly List<SocialNetwork> _socialNetworks = [];

    private VolunteerDetails(){}
    private VolunteerDetails(
        List<Requisite> requisites, 
        List<SocialNetwork> socialNetworks)
    {
        _requisites = requisites;
        _socialNetworks = socialNetworks;
    }
    
    public IReadOnlyCollection<SocialNetwork> SocialNetworks => _socialNetworks.AsReadOnly();
    public IReadOnlyCollection<Requisite> Requisites => _requisites.AsReadOnly();
    
    
    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => _socialNetworks.AddRange(socialNetworks);
    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void UpdateRequisites(List<Requisite> requisites)
    { 
        _requisites.Clear();
        _requisites.AddRange(requisites);
    }

    public void UpdateSocialNetworks(List<SocialNetwork> socialNetworks)
    {
        _socialNetworks.Clear();
        _socialNetworks.AddRange(socialNetworks);
    }
    public static Result<VolunteerDetails> Create(
        List<Requisite>? requisites, 
        List<SocialNetwork>? socialNetworks)
    {
        var details = new VolunteerDetails(
           requisites ?? [],
            socialNetworks ?? []);
        return Result<VolunteerDetails>.Success(details);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return _requisites;
        yield return _socialNetworks;
    }
}